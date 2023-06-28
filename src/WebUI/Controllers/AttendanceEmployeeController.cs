using MediatR;
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

namespace WebUI.Controllers;
public class AttendanceEmployeeController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public AttendanceEmployeeController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    [Route("/AttendanceEmployee")]
    public async Task<IActionResult> Create(DateTime tempNow)
    {
        //lấy xem coi ngày đó có làm ko.

        //lấy là Ktr IP wifi
        var urlExteranlAPI = string.Format("https://api-bdc.net/data/client-ip");
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
        //var now = DateTime.Now;
        var now = tempNow;
        var time = now.TimeOfDay;

        //lấy cấu hình ca làm:
        var listShift = await Mediator.Send(new GetListShiftRequest { });
        TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
        TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
        TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
        TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

        //lấy user
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);

        //bắt dầu phân loại ca.
        if (time >= start1 && time < end1)
        {
            try
            {
                var attendace = await Mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning, userId = user.Id });
                return BadRequest("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
            }
            catch (Exception)
            {
                try
                {
                    var Attendance = await Mediator.Send(new CreateAttendanceCommand
                    {
                        ApplicationUserId = user.Id,
                        Day = now,
                        StartTime = now,
                        ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning
                    });
                    return Ok("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!");
                }
                catch (Exception ex)
                {
                    return BadRequest("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                }
            }
        }
        else if (time >= end1 && time < start2)
        {
            try
            {
                await Mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                return Ok("Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //chấm công kết ca 1.
        }
        else if (time >= start2 && time < end2)
        {
            try
            {
                var attendace = await Mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon, userId = user.Id });
                return BadRequest("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
            }
            catch (Exception)
            {
                try
                {
                    var Attendance = await Mediator.Send(new CreateAttendanceCommand
                    {
                        ApplicationUserId = user.Id,
                        Day = now,
                        StartTime = now,
                        ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon

                    });
                    return Ok("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!");
                }
                catch (Exception ex)
                {
                    return BadRequest("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                }
            }
        }
        else if (time >= end2 && time <= TimeSpan.Parse("23:30:00"))
        {
            //chấm công kết ca 2
            try
            {
                await Mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Afternoon });
                return Ok("Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            return BadRequest("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
        }
        /*
                try
                {
                    shift = await Mediator.Send(new GetShiftDayByShiftRequest { AttendanceTime = now });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }*/

        /*try
        {
            var Attendance = await Mediator.Send(new CreateAttendanceCommand
            {
                ApplicationUserId = model.ApplicationUserId,
                Day = model.Day,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                ShiftEnum = model.ShiftEnum,

            });
            return Ok("Tạo tham gia thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Chấm công ngày "+DateTime.Now.ToString("dd/MM/yyyy")+" thất bại!");
        }*/
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

}
