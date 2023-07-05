using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Identity;
using mentor_v1.Infrastructure.Files;
using mentor_v1.Infrastructure.Identity;
using mentor_v1.Infrastructure.Persistence;
using mentor_v1.Infrastructure.Persistence.Interceptors;
using mentor_v1.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace mentor_v1.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("mentor_v1Db"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(GetConnection(configuration, env),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        services.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = true);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
        
        services.SetOptions(configuration);

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ReturnUrlParameter = "redirectUrl";
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
            {
                o.ClientId = "448432362228-njga4nde3b5255dsmnm1me16cakd2ul6.apps.googleusercontent.com";
                o.ClientSecret = "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";
                o.AccessDeniedPath = "/Forbidden/";
                o.ReturnUrlParameter = "RedirectUrl";
                o.SaveTokens = true;

                o.SignInScheme = IdentityConstants.ExternalScheme;

                o.Events.OnCreatingTicket = OnCreatingTicket;
            });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                //.AddAuthenticationSchemes(
                //    CookieAuthenticationDefaults.AuthenticationScheme‌​,
                //    GoogleDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("admin", policy => policy
                .Combine(options.DefaultPolicy)
                .RequireRole("Administrator")
                .Build());
            options.AddPolicy("member", policy => policy
                .Combine(options.DefaultPolicy)
                .RequireRole("Member")
                .Build());

        });

        return services;
    }

    private static string GetConnection(IConfiguration configuration, IWebHostEnvironment env)
    {
#if DEVELOPMENT
        return configuration.GetConnectionString("DefaultConnection") 
            ?? throw new Exception("DefaultConnection not found");
#else
        return configuration[$"ConnectionStrings:{env.EnvironmentName}"]
            ?? throw new Exception($"ConnectionStrings:{env.EnvironmentName} not found");
#endif
    }

    private static void SetOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CookieOptions>(options =>
        {
            options.IsEssential = true;
            options.SameSite = SameSiteMode.Lax;
            options.Path = "/";
            options.HttpOnly = true;
        });
    }

    private static async Task OnCreatingTicket(OAuthCreatingTicketContext ctx)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var manager = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

                var email = ctx.Principal.Claims.First(x => x.Type == ClaimTypes.Email).Value;

                var isUserExisted = await manager.FindByEmailAsync(email) != null;
                if (isUserExisted) return;

                var user = new ApplicationUser();
                user.UserName = ctx.Principal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                user.Email = email;

                var result = await manager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors.Select(x => x.Description)));
                result = await manager.AddToRoleAsync(user, "Member");
                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors.Select(x => x.Description)));
                result = await manager.AddClaimsAsync(user, ctx.Principal.Claims);
                if (!result.Succeeded)
                    throw new Exception(string.Join("\n", result.Errors.Select(x => x.Description)));

                scope.Complete();
            }
            finally { scope.Dispose(); }
        }
    }
}
