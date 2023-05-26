
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
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

/*

    [HttpGet]  // lấy danh sách employee
    public async Task<IActionResult> Index(int pg = 1)
    {
        var listEmployee = await Mediator.Send(new GetListEmployeeRequest { Page = pg, Size = 20 });
        return Ok(listEmployee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string username, string email, string role, string Fullname , string Address , string Image , string Phone , string IdentityNumber, string BankAccountNumber , string BankAccountName , string BankName, DateTime BirthDay)
    {

        if (!role.Equals("Employee") && !role.Equals("Manager") && !role.Equals("Staff"))
        {

            return BadRequest("Role không tồn tại!");
        }
        var result = await _identityService.CreateUserAsync(username, email, "Employee1!");

        if (result.Result.Succeeded)
        {
            var user = await _identityService.FindUserByEmailAsync(email);
            
                var addRoleResult = await _userManager.AddToRoleAsync(user, role);
                if(addRoleResult.Succeeded)
                {
                    try
                    {
                        var employee = await Mediator.Send(new CreateEmployeeCommand
                        {
                            Fullname = Fullname,
                            Image = Image,
                            Address = Address,
                            Phone = Phone,
                            IdentityNumber = IdentityNumber,
                            BirthDay = BirthDay,
                            BankAccountName = BankAccountName,
                            BankAccountNumber = BankAccountNumber,
                            BankName = BankAccountName,
                            ApplicationUserId = user.Id,

                        });
                        return Ok(employee);
                    }
                    catch (ValidationException ex)
                    {
                        await _userManager.DeleteAsync(user);
                        var errorsDiction = new Dictionary<string, string[]>(ex.Errors);
                        return BadRequest(errorsDiction);
                    }
                                 
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
    }*/

}
