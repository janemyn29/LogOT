using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class AuthController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    private readonly IApplicationDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager, IIdentityService identityService, IApplicationDbContext context)
    {
        _userManager = userManager;
        _identityService = identityService;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(string email, string password)
    {
        try
        {
            var result = await _identityService.AuthenticateAsync(email, password);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest();

            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}
