using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using WebUI.Services;
using WebUI.Filters;
using NToastNotify;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;

namespace WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddAppLogging(this IServiceCollection services)
    {
        // Config logging
        var loggerFactory = new LoggerFactory()
            .AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.File("./logs/log", rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext()
            .CreateLogger());
        services.AddSingleton<ILoggerFactory>(loggerFactory);

        return services;
    }

    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddDistributedMemoryCache();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
        {
            ProgressBar = true,
            PositionClass = ToastPositions.BottomRight,
            PreventDuplicates = true,
            CloseButton = true,
            TimeOut = 8000
        });
        services.AddRazorPages();

        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.ApiKey,
                Description = "Put \"Bearer {token}\" your JWT Bearer token on textbox below!",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
            };
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    jwtSecurityScheme,
                    new List<string>()
                }
            });
        });

        return services;
    }
}
