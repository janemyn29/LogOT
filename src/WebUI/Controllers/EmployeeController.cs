using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Employee.Queries.GetEmployee;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class EmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public EmployeeController(UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    //Quản lý employee của staff


    
    [HttpGet]  // lấy danh sách employee
    public async Task<IActionResult> Index(int pg=1)
    {
        var listEmployee = await Mediator.Send(new GetListEmployeeRequest { Page = pg, Size = 20 });
        return Ok(listEmployee);
    }


    [HttpPost]
    public async Task<IActionResult> Create(string username, string email)
    {
            var result = await _identityService.CreateUserAsync(username, email, "Employee1!");

        if (result.Result.Succeeded)
        {
            return Ok();

        }
        else
        {
            return BadRequest(result.Result.Errors);
        }

    }
}
