using System.IdentityModel.Tokens.Jwt;
using System.Net;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Common.PagingUser;
using mentor_v1.Application.ConfigWifis.Queries.GetByRelatedObject;
using mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowanceWithRelativeObject;
using mentor_v1.Application.Dependent.Commands.CreateDependent;
using mentor_v1.Application.Dependent.Queries;
using mentor_v1.Application.EmployeeAllowance.Queries;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
using mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;
using mentor_v1.Application.SkillEmployee.Queries;
using mentor_v1.Application.SkillEmployee.Queries.GetSkillEmployee;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebUI.Models;
using WebUI.Services.AttendanceServices;

namespace WebUI.Controllers.Employee;
public class EmpController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IAttendanceService _attendance;
    private readonly IApplicationDbContext _context;

    public EmpController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IAttendanceService attendanceService, IApplicationDbContext context)
    {
        _userManager = userManager;
        _configuration = configuration;
        _attendance = attendanceService;
        _context = context;
    }

    //Attendance

    [HttpGet]
    [Authorize(Roles = "Employee")]
    [Route("/Emp/AttendanceEmployee")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var listAttendance = await Mediator.Send(new GetListAttendanceByUserRequest { Page = pg, Size = 40, UserId = user.Id });
        return Ok(listAttendance);
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    [Route("/Emp/AttendanceEmployee/Create")]
    public async Task<IActionResult> Create(/*DateTime tempNow*/)
    {
        //lấy configday xem coi ngày đó có làm ko.

        string ip = GetIPWifi();
        if (ip == null)
        {
            return BadRequest("Vui lòng kiểm tra lại kết nối Wifi chấm công để thực hiện chấm công!");
        }
        var IpWifi = JsonConvert.DeserializeObject<IpModel>(ip);

        try
        {
            var defaultWIfi = await Mediator.Send(new GetConfigWifiByIpRequest { Ip = IpWifi.ipString });
        }
        catch (Exception)
        {
            return BadRequest("Vui lòng kiểm tra lại kết nối Wifi chấm công để thực hiện chấm công!");
        }
        //kết thúc ktr IP wify

        ShiftConfig shift;
        var now = DateTime.Now;
        //var now = tempNow;

        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        try
        {
            string result = null;
            //test result: Morning (pass: đã test kỹ)
            //test result: Full (pass: đã test )
            var annualDay = await Mediator.Send(new GetAnnualByDayRequest { Date = now });
            if (annualDay.ShiftType == ShiftType.Full)
            {
                result = await _attendance.AttendanceFullDay(now, user);
            }
            else if (annualDay.ShiftType == ShiftType.Morning)
            {
                result = await _attendance.AttendanceMorningOnly(now, user);
            }
            else if (annualDay.ShiftType == ShiftType.Afternoon)
            {
                result = await _attendance.AttendanceAfternoonOnly(now, user);
            }
            else if (annualDay.ShiftType == ShiftType.NotWork)
            {
                result = await _attendance.AttendanceNotWork(now, user);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    [Authorize(Roles = "Employee")]
    [Route("/Emp/AttendanceEmployee/Filter")]
    public async Task<IActionResult> Filter(DateTime FromDate, DateTime ToDate, int pg = 1)
    {
        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var listAttendance = await Mediator.Send(new GetListAttendanceByUserNoPaging { UserId = user.Id });
        var finalList = listAttendance.Where(x => x.Day.Date >= FromDate.Date && x.Day.Date <= ToDate.Date).ToList();
        var page = await PagingAppUser<AttendanceViewModel>.CreateAsync(finalList, pg, 40);
        var model = new AttendanceFilterViewModel();
        model.list = page;
        model.FromDate = FromDate.Date;
        model.ToDate = ToDate.Date;
        return Ok(model);
    }

    [NonAction]
    public string GetJwtFromHeader()
    {
        var httpContext = HttpContext.Request.HttpContext;
        if (httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"];
            if (authorizationHeader.ToString().StartsWith("Bearer "))
            {
                return authorizationHeader.ToString().Substring("Bearer ".Length);
            }
        }
        return null;
    }
    [NonAction]
    public string GetUserName()
    {
        string jwt = GetJwtFromHeader();
        if (jwt == null)
        {
            return null;
        }

        // Giải mã JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _configuration["JWT:ValidAudience"],
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:SecrectKey"]))
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, validationParameters, out _);
            return claimsPrincipal.Identity.Name;

        }
        catch (Exception ex)
        {
            // Xử lý lỗi giải mã JWT
            return null;
        }
    }



    [NonAction]
    public string GetIPWifi()
    {
        //lấy là Ktr IP wifi
        var urlExteranlAPI = string.Format("https://api-bdc.net/data/client-info");
        WebRequest request = WebRequest.Create(urlExteranlAPI);
        request.Method = "GET";
        HttpWebResponse response = null;
        response = (HttpWebResponse)request.GetResponse();

        string ip = null;
        using (Stream stream = response.GetResponseStream())
        {
            StreamReader sr = new StreamReader(stream);
            ip = sr.ReadToEnd();
            sr.Close();
        }
        if (ip == null)
        {

            return null;
        }
        return ip;
    }



    //Infor
    [Authorize(Roles = "Employee")]
    [HttpGet]
    [Route("/Emp/Infor")]
    public async Task<IActionResult> Infor()
    {
        try
        {
            var username = GetUserName();
            var user = await _userManager.Users.Include(x => x.Position).FirstOrDefaultAsync(c => c.UserName.Equals(username));
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest("Đã xảy ra lỗi, vui lòng truy cập lại sau!");
        }
    }

    //contract 
    [Authorize(Roles = "Employee")]
    [HttpGet]
    [Route("/Emp/ListContract")]

    public async Task<IActionResult> GetListcontract( int pg = 1)
    {
        var username = GetUserName();
        try
        {
            var user = await _userManager.FindByNameAsync(username);

            var list = await Mediator.Send(new GetEmpContractByEmpRequest { Username = username, page = pg, size = 20 });
            /*            foreach (var item in list.Items)
                        {
                            item.ApplicationUser = null;
                        }*/
            DefaultModel<PaginatedList<EmpContractViewModel>> repository = new DefaultModel<PaginatedList<EmpContractViewModel>>();
            repository.User = user;
            repository.ListItem = list;
            return Ok(repository);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Employee")]
    [Route("/Emp/ContractDetail")]
    public async Task<IActionResult> GetDetailContractByContractCode(string code)
    {
        try
        {
            var username = GetUserName();
            var Contract = await Mediator.Send(new GetEmpContractByCodeRequest { code = code });
            if(Contract.ApplicationUser.UserName.Equals(username))
            {
                return Ok(Contract);
            }
            else
            {
                return BadRequest("Bạn không có quyền truy cập vào hợp đồng này!");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    //Skill
    #region Get Skill
    [HttpGet]
    [Route("/Emp/GetListSkill")]
    public async Task<IActionResult> GetSkillEmployee(int page)
    {
        try
        {
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var result = await Mediator.Send(new GetSkillByUserIdRequest { Page = page, Size = 20,  UserId = user.Id });
            return Ok(new
            {
                staus = Ok().StatusCode,
                message = "lấy danh sách thành công.",
                result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion


    //Department
    [HttpGet]
    [Route("/Emp/Department")]
    public async Task<IActionResult> GetByUser()
    {
        try
        {
            var username = GetUserName();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
            }
            var position = await Mediator.Send(new GetPositionByIdRequest { Id = user.PositionId });
            PositionModel model = new PositionModel();
            position.ApplicationUsers = null;
            model.Position = position;
            model.User = user;
            List<Subsidize> subsidizes = new List<Subsidize>();
            var list = await Mediator.Send(new GetDepartmentAllowanceByDepartmentIdRequest { Id =  position.DepartmentId });
            foreach (var item in list)
            {
                subsidizes.Add(item.Subsidize);
            }
            model.Subsidize = subsidizes;
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        }
    }

    //dependance 
    #region Get List dependance
    [HttpGet]
    [Route("/Emp/ListDependent")]
    public async Task<IActionResult> GetListDependent(int page)
    {
        try
        {
            var username = GetUserName();

            var user = await _userManager.FindByNameAsync(username);
            var result = await Mediator.Send(new GetDependantByUserId { Page = page, Size = 20, userId = user.Id});
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy danh sách thành công.",
                result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    //Allowance 
    [HttpGet]
    [Authorize(Roles = "Employee")]
    [Route("/Emp/AllowanceByContract")]
    public async Task<IActionResult> GetListAllowancetByContractCode(string code )
    {
        try
        {
            var username = GetUserName();
            var Contract = await Mediator.Send(new GetEmpContractByCodeRequest { code = code });
            var list = await Mediator.Send(new GetEmployeeAllowanceByContractId { ContractId = Contract.Id });
            List<Allowance> allowances = new List<Allowance>();
            foreach (var item in list)
            {
                allowances.Add(item.Allowance);
            }
            if (Contract.ApplicationUser.UserName.Equals(username))
            {
                return Ok(allowances);
            }
            else
            {
                return BadRequest("Bạn không có quyền truy cập vào hợp đồng này!");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    #region GetListDependent
    [HttpGet]
    [Route("/Emp/DependanceFilter")]

    public async Task<IActionResult> DependanceFilter(AcceptanceType acceptanceType )
    {
        try
        {
            var result = await Mediator.Send(new GetListDependantNoVmRequest {  AcceptanceType = acceptanceType });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy danh sách thành công.",
                result = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Create
    [HttpPost]
    [Route("/Emp/DependentCreate")]
    public async Task<IActionResult> CreateDependent(CreateDependentViewModel createDependentViewModel)
    {
        var username = GetUserName();

        var user = await _userManager.FindByNameAsync(username);
        createDependentViewModel.ApplicationUserId = user.Id;
        var validator = new CreateDepentdentCommandValidator(_context);
        var valResult = await validator.ValidateAsync(createDependentViewModel);

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
            
            var create = await Mediator.Send(new CreateDependentCommand
            {
                createDependentViewModel = createDependentViewModel
            });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Tạo thành công."
            });
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = "Không tìm thấy người dùng bạn yêu cầu!."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = "Tạo thất bại."
            });
        }

    }
    #endregion




}
