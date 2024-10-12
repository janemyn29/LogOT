
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
using mentor_v1.Application.ApplicationUser.Queries;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Common.PagingUser;
using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.DeleteEmpContract;
using mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using mentorv1.Infrastructure.Persistence.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Models;
using WebUI.Services.ContractServices;
using WebUI.Services.FileManager;

namespace WebUI.Controllers;

[Authorize(Roles = "Manager")]

public class EmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;
    private readonly IFileService _fileService;
    private readonly IApplicationDbContext _context;
    private readonly IWebHostEnvironment environment;
    private readonly IMapper _mapper;
    private readonly IContracService _contract;

    public EmployeeController(IWebHostEnvironment ev, IFileService fileService, UserManager<ApplicationUser> userManager, IIdentityService identityService, IApplicationDbContext context, IMapper mapper,IContracService  contracService)
    {
        _userManager = userManager;
        _identityService = identityService;
        _context = context;
        _fileService = fileService;
        environment = ev;
        _mapper = mapper;
        _contract = contracService;
    }

    //Quản lý employee của staff


    /*    [Authorize(Roles = "Manager")]*/
    [HttpGet]  // lấy danh sách employee
    [Route("/Employee")]

    public async Task<IActionResult> Index(int pg = 1)
    {
        var file = new FileService(environment);
        var listEmployee = await Mediator.Send(new GetListUserRequest { Page = 1, Size = 20 });
        /*foreach (var item in listEmployee.Items)
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
        }*/
        return Ok(listEmployee);
    }

    [HttpPost]
    [Route("/CreateManagerAccount")]
    public async Task<IActionResult> CreateManagerAccount([FromBody] ManagerMOdel newManager)
    {

        try
        {
            if(newManager.BirthDay.Date.AddYears(18).Date > DateTime.Now.Date) {
                return BadRequest("Phải lớn hơn 18 tuổi!");
                    }
            var administrator = new ApplicationUser { UserName = newManager.Username, Email = newManager.Email, Fullname = newManager.Fullname, Image = newManager.Image, Address = newManager.Address, IdentityNumber = "0", BirthDay = newManager.BirthDay, BankAccountNumber = "0", BankAccountName = "0", BankName = "0", GenderType =newManager.GenderType, PositionId = newManager.PositionId, IsMaternity = false };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
               var result =  await _userManager.CreateAsync(administrator, "Manager1!");
                
                if(result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(administrator, new[] { "Manager" });
                    return Ok("Tạo tài khoản quản lý thành công!");

                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return BadRequest("Tên đăng nhập này đã tồn lại!");
            }

            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    /*    [Authorize(Roles = "Manager")]*/
    [HttpGet]  // lấy danh sách employee
    [Route("/GetListManagerAccount")]

    public async Task<IActionResult> GetListManagerAccount(int pg = 1)
    {
        var ListApplicationUser = await _userManager.GetUsersInRoleAsync("Manager");
        var list = ListApplicationUser.OrderBy(x => x.Fullname).ToList();

        // Paginate data
        var page = await PagingAppUser<ApplicationUser>
            .CreateAsync(list, pg, 20);
        return Ok(page);
    }


    [HttpPut]
    [Route("/Employee/LockAccount")]
    public async Task<IActionResult> LockAccount(string userId)
    {
        try
        {
            var result = await Mediator.Send(new UpdateLockoutAccount() { id = userId });
            return Ok("Khóa tài khoản thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Khóa tài khoản thất bại!");
        }
    }

    [HttpPut]
    [Route("/Employee/UnlockAccount")]
    public async Task<IActionResult> UnlockAccount(string userId)
    {
        try
        {
            var result = await Mediator.Send(new UpdateUnlockAccount() { id = userId });
            return Ok("Mở khóa tài khoản thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Mở khóa tài khoản thất bại!");
        }
    }


    [HttpPost]
    [Route("/Employee/Create")]
    public async Task<IActionResult> Create([FromBody] EmployeeModel newEmp )
    {
        Guid contractIid = Guid.NewGuid();
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        var model = _mapper.Map<UserViewModel>(newEmp);
        var contract = _mapper.Map<CreateEmployeeContractCommand>(newEmp);

        var errors = await _contract.CheckValidatorCreateEmployee(contract);
        var validator = new CreateUseCommandValidator(_context, _userManager);
        var valResult = await validator.ValidateAsync(model);

         if (!newEmp.Role.Equals("Employee") && !newEmp.Role.Equals("Manager"))
        {

            return BadRequest("Role không tồn tại!");
        }
        if (valResult.Errors.Count != 0)
        {
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
        }
        if (errors != null && errors.Count > 0)
        {
            return BadRequest(errors);
        }
        try
        {/*
                string fileResult = _fileService.SaveImage(model.Image);*/
            var result = await _identityService.CreateUserAsync(model.Username, model.Email, "Employee1!", model.Fullname, model.Image, model.Address, model.IdentityNumber, model.BirthDay, model.BankAccountNumber, model.BankAccountName, model.BankName, model.PositionId, model.GenderType, model.IsMaternity, mentor_v1.Domain.Enums.WorkStatus.StillWork,model.PhoneNumber);

            if (result.Result.Succeeded)
            {
                var user = await _identityService.FindUserByEmailAsync(model.Email);
                try
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, newEmp.Role);
                    
                   
                    if (addRoleResult.Succeeded)
                    {
                        try
                        {

                            /*            var filePath = await _fileService.UploadFile(model.File);*/
                            contractIid = await Mediator.Send(new CreateEmployeeContractCommand
                            {
                                Username = model.Username,
                                ContractCode = contract.ContractCode,
                                StartDate = contract.StartDate,
                                EndDate = contract.EndDate,
                                BasicSalary = contract.BasicSalary,
                                File = contract.File,
                                Job = contract.Job,
                                ContractType = contract.ContractType,
                                PercentDeduction = contract.PercentDeduction,
                                SalaryType = contract.SalaryType,
                                isPersonalTaxDeduction = contract.isPersonalTaxDeduction,
                                InsuranceAmount = contract.InsuranceAmount,
                                InsuranceType = contract.InsuranceType,
                                AllowanceId = contract.AllowanceId,
                            });

                        }
                        catch (Exception ex)
                        {
                            await _userManager.DeleteAsync(user);
                            return BadRequest(ex.Message);
                        }
                        return Ok(user);
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        return BadRequest("Thêm Role bị lỗi");
                    }
                }
                catch (Exception)
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    /*
    [Authorize(Roles = "Manager")]*/
    [HttpPut]
    [Route("/Employee/Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        var validator = new UpdateUserCommandValidator(_context, _userManager);
        var valResult = await validator.ValidateAsync(model);
        try
        {
            var position = await Mediator.Send(new GetPositionByIdRequest { Id = model.PositionId });
        }
        catch (Exception ex)
        {

            return NotFound(ex.Message);
        }
        if (valResult.Errors.Count != 0)
        {

            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            return BadRequest(errors);

        }

        try
        {
            /*                string fileResult = _fileService.SaveImage(model.Image);*/
            var result = await Mediator.Send(new UpdateUserCommand { model = model });
            return Ok("Cập nhật thông tin thành công!");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    /*
    [Authorize(Roles = "Manager")]*/
    [HttpPut]
    [Route("/Employee/UpdateEmail")]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateMailModel model)
    {
        if (model.UserId == null||model.NewEmail==null)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        

        try
        {
            /*                string fileResult = _fileService.SaveImage(model.Image);*/
            var result = await Mediator.Send(new UpdateEmailCommand { model = model });
            return Ok("Cập nhật thông tin thành công!");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [Route("/Employee/Quit")]
    public async Task<IActionResult> QuitEmployee(string userId)
    {
        if (userId == null)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        try
        {
            var result = await Mediator.Send(new UpdateUserWorkStatusRequest { id = userId });
            return Ok("Cập nhật thông tin thôi việc cho nhân viên thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [Route("/Employee/UpdateMaternity")]
    public async Task<IActionResult> UpdateMaternity(string userId)
    {
        if (userId == null)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        try
        {
            var result = await Mediator.Send(new UpdateMaterityStatus { id = userId, IsMaternity =false });
            return Ok("Cập nhật thông tin mang thai cho nhân viên thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    /*
    [Authorize(Roles = "Manager")]*/
    [HttpGet]
    [Route("/Employee/GetDetailEmployee")]
    public async Task<IActionResult> GetDetailEmployee(string username)
    {
    
        try
        {
            var result = await _userManager.FindByNameAsync(username);
            return Ok(result);

        }
        catch (Exception ex)
        {
            return BadRequest("Không tìm thấy nhân viên có tên đăng nhập là  "+ username + "!");
        }

    }

    [HttpGet]
    [Route("/Employee/Search")]
    public async Task<IActionResult> Search(string Keyword, int pg= 1)
    {
        var result = _userManager.Users.Where(x=>x.UserName.ToLower().Contains(Keyword.ToLower()) || x.Email.ToLower().Contains(Keyword.ToLower())|| x.Fullname.ToLower().Contains(Keyword.ToLower())).OrderBy(x => x.Fullname).ToList();
        var page = await PagingAppUser<ApplicationUser>.CreateAsync(result, pg,20);
        var model = new DefalutSearchModel<PagingAppUser<ApplicationUser>>();
        model.DefautList = page;
        model.Keyword = Keyword;
        return Ok(model);
    }
}
