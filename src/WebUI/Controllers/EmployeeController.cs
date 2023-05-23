using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class EmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var listUser = await _userManager.GetUsersInRoleAsync("Manager");
        return Ok(listUser);
    }
}
