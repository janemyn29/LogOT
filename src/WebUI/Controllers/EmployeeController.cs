
using System.ComponentModel.DataAnnotations;
using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Models;

namespace WebUI.Controllers;
public class EmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    private readonly IApplicationDbContext _context;

    public EmployeeController(UserManager<ApplicationUser> userManager, IIdentityService identityService, IApplicationDbContext context)
    {
        _userManager = userManager;
        _identityService = identityService;
        _context = context;
    }

    //Quản lý employee của staff



    [HttpGet]  // lấy danh sách employee
    public async Task<IActionResult> Index(int pg = 1)
    {
        var listEmployee = await Mediator.Send(new GetListUserRequest { Page=1, Size=20 });
        return Ok(listEmployee);
    }

    [HttpPost]
    [Route("Employee/Create")]
    public async Task<IActionResult> Create(string role, [FromBody] UserViewModel model)
    {
        
        if (!role.Equals("Employee") && !role.Equals("Manager") && !role.Equals("Staff"))
        {

            return BadRequest("Role không tồn tại!");
        }
        var validator = new CreateUseCommandValidator(_context,_userManager);
        var valResult = await validator.ValidateAsync(model);
        List<string> errors = new List<string>();
        bool check = false;

        var tempUser = await _userManager.Users.Where(x=>x.IdentityNumber.Equals(model.IdentityNumber)).FirstOrDefaultAsync();
        if (tempUser != null)
        {
            errors.Add("Số cccd đã tồn tại");
            check = true;
        }

        if (valResult.Errors.Count != 0)
        {
          
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            check = true;

        }
        if (check)
        {
            return BadRequest(errors);
        }
        
        var result = await _identityService.CreateUserAsync(model.Username, model.Email, "Employee1!", model.Fullname, model.Image, model.Address, model.IdentityNumber, model.BirthDay,model.BankAccountNumber, model.BankAccountName,model.BankName);

        if (result.Result.Succeeded)
        {
            var user = await _identityService.FindUserByEmailAsync(model.Email);

            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (addRoleResult.Succeeded)
            {
                return Ok(user);
            }
            else
            {
                await _userManager.DeleteAsync(user);

                return BadRequest("Thêm Role bị lỗi");

            }
        }
        else
        {
            return BadRequest(result.Result.Errors);
        }
    }

}
