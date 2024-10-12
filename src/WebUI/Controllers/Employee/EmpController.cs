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
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;
using mentor_v1.Application.Payslip.Queries.GetList;
using mentor_v1.Application.Payslip.Queries;
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
using DocumentFormat.OpenXml.Spreadsheet;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Application.Dependent;
using mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.LeaveLog.Commands.DeleteLeaveLog;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;
using mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Application.Degree.Commands.CreateDegree;

namespace WebUI.Controllers.Employee;
[Authorize(Roles = "Employee")]
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
    [Route("/Emp/AttendanceEmployee")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var listAttendance = await Mediator.Send(new GetListAttendanceByUserRequest { Page = pg, Size = 40, UserId = user.Id });
        return Ok(listAttendance);
    }

    [HttpGet]
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
    [Route("/Emp/AttendanceEmployee/CreateAttendanceForMobileOnly")]
    public async Task<IActionResult> CreateAttendanceForMobileOnly(/*DateTime tempNow*/ string ip)
    {
        //lấy configday xem coi ngày đó có làm ko.
        //string ip = GetIPWifi();
        if (ip == null)
        {
            return BadRequest("Vui lòng kiểm tra lại kết nối Wifi chấm công để thực hiện chấm công!");
        }
        //var IpWifi = JsonConvert.DeserializeObject<IpModel>(ip);

        try
        {
            var defaultWIfi = await Mediator.Send(new GetConfigWifiByIpRequest { Ip = ip });
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

    [HttpGet]
    [Route("/Emp/AttendanceEmployee/GetAttendanceCurrentDay")]
    public async Task<IActionResult> GetAttendanceCurrentDay()
    {
        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var listAttendance = await Mediator.Send(new GetListAttendanceByUserNoPaging { UserId = user.Id });
        var finalList = listAttendance.Where(x => x.Day.Date >= DateTime.Now.Date && x.Day.Date <= DateTime.Now.Date).ToList();
        if(finalList ==null||finalList.Count == 0)
        {
            return BadRequest("Hôm nay bạn chưa thực hiện chấm công");
        }
        return Ok(finalList);
    }


    [HttpGet]
    [Route("/Emp/AttendanceEmployee/AttendantRegulations")]
    public async Task<IActionResult> AttendantRegulations()
    {
        var Regulations = new AttendantRegulations();
        var listShift = await Mediator.Send(new GetListShiftViewmodel { });
        Regulations.Title = "Lưu ý:";
        Regulations.Morning = "Thời gian chấm công ca Sáng từ : " + listShift.ElementAt(0).StartTime.Value.AddMinutes(-30).ToString("HH:mm:ss") + " tới " + listShift.ElementAt(1).StartTime.Value.AddMinutes(-30).AddSeconds(-1).ToString("HH:mm:ss");
        Regulations.Afternoon = "Thời gian chấm công ca Chiều từ : " + listShift.ElementAt(1).StartTime.Value.AddMinutes(-30).ToString("HH:mm:ss") + " tới " + DateTime.Parse("2023-01-10 23:29").ToString("HH:mm:ss");
        return Ok(Regulations);
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

    //contract 
    [HttpGet]
    [Route("/Emp/GetListContract2")]
    public async Task<IActionResult> GetListContract2(int pg = 1)
    {
        var username = GetUserName();
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            var list = await Mediator.Send(new GetEmpContractByEmpRequest { Username = username, page = pg, size = 20 });
            foreach (var item in list.Items)
            {
                item.ApplicationUser = null;
            }
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
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
    public async Task<IActionResult> GetListDependent(int page = 1)
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

    #region Create
    [HttpPost]
    public async Task<IActionResult> CreateDegree(CreateDegreeViewModel createDegreeViewModel)
    {
        var validator = new CreateDegreeCommadValidator(_context);
        var valResult = await validator.ValidateAsync(createDegreeViewModel);

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
            var create = await Mediator.Send(new CreateDegreeCommand
            {
                createDegreeViewModel = createDegreeViewModel
            });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Tạo thành công."
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

    #region GetListDependent
    [HttpGet]
    [Route("/Emp/DependanceFilter")]
    public async Task<IActionResult> DependanceFilter(AcceptanceType acceptanceType)
    {
        try
        {
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var result = await Mediator.Send(new GetListDependantNoVmRequest { AcceptanceType = acceptanceType });
            var temp = result.Where(x => x.ApplicationUserId.ToLower().Equals(user.Id.ToLower())).ToList();
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy danh sách thành công.",
                result = temp
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


    

    #region Create
    [HttpPost]
    [Route("/Emp/DependentCreate")]
    public async Task<IActionResult> CreateDependent(DependantModel DependentViewModel)
    {
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        CreateDependentViewModel createDependentViewModel = new CreateDependentViewModel();
        createDependentViewModel.BirthDate = DependentViewModel.BirthDate;
        createDependentViewModel.Relationship = DependentViewModel.Relationship;
        createDependentViewModel.Desciption = DependentViewModel.Desciption;
        createDependentViewModel.Name = DependentViewModel.Name;
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


    //Leave log
    #region [getListForEmployee]
    
    [HttpGet]
    [Route("/Emp/LeaveLog")]
    public async Task<IActionResult> GetListLeaveLogByEmployeeId(int pg = 1)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var listOTLog = await Mediator.Send(new GetListLeaveLogByUserIdRequest() { userId = user.Id, Page = pg, Size = 20 });
            return Ok(listOTLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách nghỉ làm");
        }
    }
    #endregion

    #region getLeaveLogById
    [HttpGet()]
    [Route("/Emp/GetLeaveLogById")]
    public async Task<IActionResult> GetLeaveLogById(Guid id)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var OTLog = await Mediator.Send(new GetLeaveLogByIdRequest() { Id = id });
            if (OTLog.ApplicationUserId.ToLower().Equals(user.Id.ToLower()))
            {
                return Ok(OTLog);

            }
            else
            {
                return BadRequest(new
                {
                    Id = id,
                    message = "Bạn không có quyền truy cập vào yêu cầu nghỉ làm ngày!"
                });
            }
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                Id = id,
                message = "Không tìm thấy yêu cầu nghỉ phép cần truy vấn"
            });
        }
    }
    #endregion
    #region [GetLeaveLogFilterByStatus]
    [HttpGet]
    [Route("/Emp/GetListLeaveLogFilterByStatus")]
    public async Task<IActionResult> GetLeaveLogFilterByStatus(LogStatus logStatus)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username); 
            var listLeaveLog = await Mediator.Send(new GetListLeaveLogNoPG { });
            var result = listLeaveLog.Where(x => x.Status.Equals(logStatus.ToString()) && x.ApplicationUserId.ToLower().Equals(user.Id.ToLower())).ToList();
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách nghỉ");
        }
    }
    #endregion

    #region [create]
    [Authorize(Roles = "Employee")]
    [HttpPost]
    [Route("/Emp/CreateLeaveLog")]
    public async Task<IActionResult> CreateLeaveLog([FromBody] CreateLeaveLogViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new CreateLeaveLogCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);

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
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var create = Mediator.Send(new CreateLeaveLogCommand() { user = user, createLeaveLogViewModel = model });
            return Ok(new
            {
                id = create,
                message = "Khởi tạo thành công"
            });
        }
        catch (Exception e)
        {
            return BadRequest("Khởi tạo thất bại: " + e.Message);
        }
    }
    #endregion
    #region deleteLeaveLog
    [Authorize(Roles = "Employee")]
    [HttpPut]
    [Route("/Emp/DeleteLeaveLog")]
    public async Task<IActionResult> DeleteLeaveLog(Guid id)
    {
        if (id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");

        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }

        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var currentLeaveLog = await Mediator.Send(new GetLeaveLogByIdRequest() { Id = id });
            if(currentLeaveLog != null && !currentLeaveLog.ApplicationUserId.ToLower().Equals(user.Id.ToLower()) ){
                return BadRequest("Bạn không được cấp quyền để cập nhật yêu cầu này!");
            }
            if (currentLeaveLog.LeaveDate > DateTime.Now)
            {
                if (currentLeaveLog.Status.ToString().ToLower().Equals("request"))
                {
                    var delete = await Mediator.Send(new DeleteLeaveLogCommand() { Id = id });
                    return Ok("Cập nhật yêu cầu thành công");
                }
                else
                {
                    return BadRequest( "Không thể cập nhật, yêu cầu đã được xử lý");
                }
            }
            else
            {
                return BadRequest("Không thể xóa, ngày yêu cầu đã qua thời gian hiện tại");
            }
        }
        catch (NotFoundException)
        {
            return BadRequest("Không tìm thấy id theo yêu cầu");
        }
        catch (Exception e)
        {
            return BadRequest("Cập nhật không thành công: " + e.Message);
        }
    }
    #endregion
    //Payslip 

    [HttpGet]
    [Route("/Emp/GetListPayslip")]
    public async Task<IActionResult> GetListPayslip(int pg = 1)
    {
        //lấy user từ username ở header
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var list = await Mediator.Send(new GetListPaySlipByUserReuqest { Page = pg, Size = 30, userId = user.Id });
        return Ok(list);
    }

    [HttpGet]
    [Route("/Emp/GetListPayslipByUserOrMonthOrBoth")]
    public async Task<IActionResult> GetListPayslipByUserOrMonthOrBoth(int? month, int? year)
    {
        //lấy user từ username ở header
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var userId = user.Id;
        var list = await Mediator.Send(new GetListPayslipNoPg { });
        List<PaySlip> final = new List<PaySlip>();
        if (userId != null && month != null && year != null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) && x.ToTime.Month == month && x.ToTime.Year == year).ToList();

        }
        else if (userId != null && month != null && year == null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) && x.ToTime.Month == month).ToList();

        }
        else if (userId != null && month == null && year != null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) && x.ToTime.Year == year).ToList();
        }
        else if (userId == null && month != null && year != null)
        {
            final = list.Where(x => x.ToTime.Month == month && x.ToTime.Year == year).ToList();
        }
        else if (userId != null && month == null && year == null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower())).ToList();
        }
        else if (userId == null && month == null && year != null)
        {
            final = list.Where(x => x.ToTime.Year == year).ToList();
        }
        else if (userId == null && month != null && month == null)
        {
            final = list.Where(x => x.ToTime.Month == month).ToList();
        }
        else
        {
        }
        return Ok(final);
    }


    [HttpGet]
    [Route("/Emp/GetDetailPayslip")]
    public async Task<IActionResult> GetDetailPayslip(Guid id)
    {
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var item = await Mediator.Send(new GetPaySlipRequets { Id = id });
        if (item.EmployeeContract.ApplicationUserId.ToLower().Equals(user.Id.ToLower()))
        {
            return Ok(item);

        }
        else
        {
            return BadRequest("Bạn không có quyền truy cập vào bảng lương này!");
        }
    }


    //logOT
    #region [getListForEmployee]
    [Authorize(Roles = "Employee")]
    [HttpGet]
    [Route("/Emp/GetOvertimeLog")]
    public async Task<IActionResult> GetOvertimeLogByEmployeeId(int pg=1)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);


            var listOTLog = await Mediator.Send(new GetOvertimeLogByUserIdRequest() { id = user.Id, Page = pg, Size = 10 });
            return Ok(listOTLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách tăng ca");
        }
    }
    #endregion

    #region getOvertimeLogById
    [HttpGet]
    [Route("/Emp/GetOvertimeLogById")]
    public async Task<IActionResult> GetOvertimeLogById(Guid id)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var role = await _userManager.GetRolesAsync(user);
            if (role == null) throw new Exception("user chưa có role");

            var OTLog = await Mediator.Send(new GetOvertimeLogByIdRequest() { Id = id, user = user, Role = role.FirstOrDefault() });
            return Ok(OTLog);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy id cần truy vấn");
        }
    }
    #endregion

    #region approveOvertimeRequest
    [Authorize(Roles = "Employee")]
    [HttpPut]
    [Route("/Emp/UpdateStatusOvertimeLogRequest")]
    public async Task<IActionResult> UpdateStatusOvertimeLogRequest([FromBody] UpdateOvertimeLogRequestStatusCommand model)
    {
        //lấy user từ username ở header
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);

        var listManager = await _userManager.GetUsersInRoleAsync("Manager");


        if (model.Id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");
        try
        {
            foreach (var item in listManager)
            {
                var noti = await Mediator.Send(new CreateNotiCommand()
                {
                    ApplicationUserId = item.Id,
                    Title = "Thông báo về việc xác nhận yêu cầu OT của nhân viên",
                    Description = "Nhân viên: " + user.Fullname + " \n" +
                    " đã xác nhận: " + model.status.ToString() + "\n" +
                    "vào lúc: " + DateTime.Now
                });
            }
            if (model.status== LogStatus.Approved)
            {
                var update = await Mediator.Send(new UpdateOvertimeLogRequestStatusCommand()
                {
                    Id = model.Id,
                    status = mentor_v1.Domain.Enums.LogStatus.Approved
                });
                return Ok("Xác nhận yêu cầu thành công");
            }
            else if (model.status == LogStatus.Cancel)
            {
                var update = await Mediator.Send(new UpdateOvertimeLogRequestStatusCommand()
                {
                    Id = model.Id,
                    status = mentor_v1.Domain.Enums.LogStatus.Cancel,
                    cancelReason = model.cancelReason
                });
                return Ok("Từ chối yêu cầu thành công");
            }
            //return Ok("Xác nhận yêu cầu thành công");
            throw new Exception();
        }
        catch (Exception)
        {
            return BadRequest("Xác nhận trạng thái yêu cầu không thành công");
        }
    }
    #endregion



    #region getOvertimeLogById
    [HttpGet]
    [Route("/Emp/GetPosition")]
    public async Task<IActionResult> GetPosition()
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var role = await _userManager.GetRolesAsync(user);
            if (role == null) throw new Exception("user chưa có role");
            var position = await Mediator.Send(new GetPositionByIdRequest() { Id= user.PositionId });
            return Ok(position);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy id cần truy vấn");
        }
    }
    #endregion
}
