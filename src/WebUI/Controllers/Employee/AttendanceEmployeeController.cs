/*using MediatR;
using mentor_v1.Application.Attendance.Commands.CreateAttendance;
using mentor_v1.Application.Attendance.Commands.UpdateAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Application.ConfigWifis.Queries.GetByRelatedObject;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Models;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Domain.Enums;
using mentor_v1.Application.Common.PagingUser;
using WebUI.Services.AttendanceServices;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;

namespace WebUI.Controllers.Employee;
public class AttendanceEmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IAttendanceService _attendance;
    private readonly IApplicationDbContext _context;

    public AttendanceEmployeeController(UserManager<ApplicationUser> userManager, IConfiguration configuration,IAttendanceService attendanceService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _attendance = attendanceService;
    }

    [HttpGet]
    [Authorize(Roles = "Employee")]
    [Route("/AttendanceEmployee")]
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
    [Route("/AttendanceEmployee/Create")]
    public async Task<IActionResult> Create(*//*DateTime tempNow*//*)
    {
        //lấy configday xem coi ngày đó có làm ko.

       string ip = GetIPWifi();
        if(ip == null)
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
            if(annualDay.ShiftType == ShiftType.Full)
            {
                result = await _attendance.AttendanceFullDay(now, user);
            }else if(annualDay.ShiftType == ShiftType.Morning)
            {
                 result = await _attendance.AttendanceMorningOnly(now, user);
            }else if(annualDay.ShiftType == ShiftType.Afternoon)
            {
                 result = await _attendance.AttendanceAfternoonOnly(now, user);
            }else if(annualDay.ShiftType == ShiftType.NotWork) {
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
    [Route("/AttendanceEmployee/Filter")]
    public async Task<IActionResult> Filter( DateTime FromDate, DateTime ToDate , int pg = 1)
    {
        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);
        var listAttendance = await Mediator.Send(new GetListAttendanceByUserNoPaging { UserId = user.Id });
        var finalList = listAttendance.Where(x=>x.Day.Date >= FromDate.Date && x.Day.Date <= ToDate.Date).ToList();
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecrectKey"]))
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
}
*/