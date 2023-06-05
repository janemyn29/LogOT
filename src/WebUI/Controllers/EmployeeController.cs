
using System.ComponentModel.DataAnnotations;
using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Models;
using WebUI.Services.FileManager;

namespace WebUI.Controllers;


public class EmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;
    private readonly IFileService _fileService;
    private readonly IApplicationDbContext _context;
    private readonly IWebHostEnvironment environment;

    public EmployeeController(IWebHostEnvironment ev, IFileService fileService,UserManager<ApplicationUser> userManager, IIdentityService identityService, IApplicationDbContext context)
    {
        _userManager = userManager;
        _identityService = identityService;
        _context = context;
        _fileService = fileService;
        environment = ev;
    }

    //Quản lý employee của staff


    [Authorize(Roles = "Manager")]
    [HttpGet]  // lấy danh sách employee
    [Route("/Employee")]

    public async Task<IActionResult> Index(int pg = 1)
    {
        var file = new FileService(environment);
        var listEmployee = await Mediator.Send(new GetListUserRequest { Page=1, Size=20 });
        foreach (var item in listEmployee.Items)
        {
            if (item.Image != null)
            {
                try
                {
                    var wwwPath = this.environment.ContentRootPath;
                    var path = Path.Combine(wwwPath, "Uploads\\", item.Image);
                    if (System.IO.File.Exists(path))
                    {
                        byte[] base64 = System.IO.File.ReadAllBytes(path);
                        item.ImageBase = Convert.ToBase64String(base64);
                    }
                   
                }
                catch (Exception ex)
                {
                }

            }
        }
        return Ok(listEmployee);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [Route("/Employee/Create")]
    public async Task<IActionResult> Create(string role, [FromForm] UserViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        if (!role.Equals("Employee") && !role.Equals("Manager") && !role.Equals("Staff"))
        {

            return BadRequest("Role không tồn tại!");
        }
        var validator = new CreateUseCommandValidator(_context,_userManager);
        var valResult = await validator.ValidateAsync(model);
       

        var tempUser = await _userManager.Users.Where(x=>x.IdentityNumber.Equals(model.IdentityNumber)).FirstOrDefaultAsync();
        if (valResult.Errors.Count != 0)
        {

            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            return BadRequest(errors);

        }
        if(model.Image!= null)
        {
            var fileResult = _fileService.SaveImage(model.Image);
            var result = await _identityService.CreateUserAsync(model.Username, model.Email, "Employee1!", model.Fullname, fileResult, model.Address, model.IdentityNumber, model.BirthDay, model.BankAccountNumber, model.BankAccountName, model.BankName, model.PositionId, model.GenderType, model.IsMaternity);

            if (result.Result.Succeeded)
            {
                var user = await _identityService.FindUserByEmailAsync(model.Email);

                var addRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (addRoleResult.Succeeded)
                {
                    //confirm email
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
        else
        {
            return BadRequest("Vui lòng không để trống hình ảnh!");
        }

    }

}
