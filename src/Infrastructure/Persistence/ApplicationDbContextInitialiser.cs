using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mentor_v1.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
        }
        var memberRole = new IdentityRole("Member");

        if (_roleManager.Roles.All(r => r.Name != memberRole.Name))
        {
            await _roleManager.CreateAsync(memberRole);
        }

        // Default users
        var member = new ApplicationUser { UserName = "member@localhost", Email = "member@localhost" };

        if (_userManager.Users.All(u => u.UserName != member.UserName))
        {
            await _userManager.CreateAsync(member, "Member1!");
            await _userManager.AddToRolesAsync(member, new[] { memberRole.Name });
        }

        var mentorRole = new IdentityRole("Mentor");

        if (_roleManager.Roles.All(r => r.Name != mentorRole.Name))
        {
            await _roleManager.CreateAsync(mentorRole);
        }

        // Default users
        var mentor = new ApplicationUser { UserName = "mentor@localhost", Email = "mentor@localhost" };

        if (_userManager.Users.All(u => u.UserName != member.UserName))
        {
            await _userManager.CreateAsync(member, "Mentor1!");
            await _userManager.AddToRolesAsync(member, new[] { mentorRole.Name });
        }
    }
}
