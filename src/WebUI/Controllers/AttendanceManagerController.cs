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

namespace WebUI.Controllers;

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
        var listAttendance = await Mediator.Send(new GetListAttendanceRequest { Page = 1, Size = 20 });
        return Ok(listAttendance);
    }
    //Manager
    //Get List attendence of user / from day to day


    ///User
    //Get list Attendance of current User
    //Get list Attendance from day to day of current User


    
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
}
