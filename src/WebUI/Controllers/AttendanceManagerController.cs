using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Attendance.Commands.CreateAttendance;
using mentor_v1.Application.Attendance.Commands.DeleteAttendance;
using mentor_v1.Application.Attendance.Commands.UpdateAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Newtonsoft.Json;
using WebUI.Models;
using mentor_v1.Application.ConfigWifis.Queries.GetByRelatedObject;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using mentor_v1.Application.Common.PagingUser;

namespace WebUI.Controllers;

[Authorize(Roles = "Manager")]
public class AttendanceManagerController : ApiControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public AttendanceManagerController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    /*[Authorize (Roles ="Manager") ]*/
    //get list department
    [HttpGet]
    [Route("/Attendance")]
    public async Task<IActionResult> index(int pg = 1)
    {
        var listAttendance = await Mediator.Send(new GetListAttendanceRequest { Page = pg, Size = 20 });
        return Ok(listAttendance);
    }
    //Manager
    //Get List attendence of user / from day to day


    ///User
    //Get list Attendance of current User
    //Get list Attendance from day to day of current User

    [HttpGet]
    [Route("/Attendance/CreateSeries")]
    public async Task<IActionResult> CreateSeries(DateTime FromDate, DateTime ToDate, string email)
    {
        var distanceDay = ToDate - FromDate;
        int distance = distanceDay.Days;
        DateTime tempDate = FromDate;
        var listUser = await _userManager.GetUsersInRoleAsync("Employee");
        foreach (var item in listUser)
        {
            if(item.WorkStatus == mentor_v1.Domain.Enums.WorkStatus.StillWork && item.Email.Equals(email))
            {
                for (int i = 0; i < distance; i++)
                {

                    var date = tempDate.AddHours(8);

                    await Mediator.Send(new CreateAttendanceManualCommand
                    {
                        ApplicationUserId = item.Id,
                        Day = tempDate,
                        StartTime = date,
                        EndTime = date.AddHours(4),

                        ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning,
                        OtHour = 0,
                        WorkHour = 4
                    });

                    await Mediator.Send(new CreateAttendanceManualCommand
                    {
                        ApplicationUserId = item.Id,
                        Day = tempDate,
                        StartTime = date.AddHours(5).AddMinutes(30),
                        EndTime = date.AddHours(9).AddMinutes(30),
                        ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon,
                        OtHour = 0,
                        WorkHour = 4
                    });
                    tempDate = tempDate.AddDays(1);
                }
            }
                
        }

        return Ok();
    }
    
    

    /*[HttpPut]
    [Route("/Attendance/Update")]
    public async Task<IActionResult> Update(UpdateAttendanceCommand model)
    {
        var validator = new UpdateAttendanceCommandValidator(_context);
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
            var Attendance = await Mediator.Send(new GetAttendanceByIdRequest { Id = model.Id });
            try
            {

                var AttendanceUpdate = await Mediator.Send(new UpdateAttendanceCommand
                {
                    Id = model.Id,
                    ApplicationUserId = model.ApplicationUserId,
                    Day = model.Day,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    ShiftEnum = model.ShiftEnum,
                });
                return Ok("Cập nhật tham gia thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy tham gia yêu cầu!");

        }
    }*/

    /* [HttpDelete]
     [Route("/Attendance/Delete")]
     public async Task<IActionResult> Delete(Guid id)
     {
         try
         {
             var result = await Mediator.Send(new DeleteAttendanceCommand { Id = id });
             return Ok("Xóa kinh nghiệm thành công!");
         }
         catch (Exception ex)
         {
             return BadRequest("Xóa kinh nghiệm thất bại!");
         }
     }*/

    [HttpGet]
    [Route("/Attendance/GetListByUser")]
    public async Task<IActionResult> GetByUser(string username, int pg = 1)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
            }
            var listAttendance = await Mediator.Send(new GetListAttendanceByApplicationUserIdRequest { Id = user.Id, Page = pg , Size = 20 });
            return Ok(listAttendance);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        }
    }

    [HttpGet]
    [Route("/Attendance/Filter")]
    public async Task<IActionResult> Filter(DateTime FromDate, DateTime ToDate, int pg = 1)
    {
        var listAttendance = await Mediator.Send(new GetListAttendanceNoVm { });
        var finalList = listAttendance.Where(x => x.Day.Date >= FromDate.Date && x.Day.Date <= ToDate.Date).ToList();
        var page = await PagingAppUser<AttendanceViewModel>.CreateAsync(finalList, pg, 40);
        var model = new AttendanceFilterViewModel();
        model.list = page;
        model.FromDate = FromDate.Date;
        model.ToDate = ToDate.Date;
        return Ok(model);
    }
}
