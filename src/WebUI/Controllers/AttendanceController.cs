using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Attendance.Commands.CreateAttendance;
using mentor_v1.Application.Attendance.Commands.DeleteAttendance;
using mentor_v1.Application.Attendance.Commands.UpdateAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class AttendanceController : ApiControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public AttendanceController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    //get list department
    [HttpGet]
    [Route("/Attendance")]
    public async Task<IActionResult> index(int pg = 1)
    {
        var listAttendance = await Mediator.Send(new GetListAttendanceRequest { Page = 1, Size = 20 });
        return Ok(listAttendance);
    }

    [HttpPost]
    [Route("/Attendance/Create")]
    public async Task<IActionResult> Create(CreateAttendanceCommand model)
    {
        var validator = new CreateAttendanceCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);
        if (valResult.Errors.Count != 0)
        {

            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage;
                errors.Add(item);
            }
            return BadRequest(errors);

        }
        try
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
            return BadRequest("Tạo tham gia thất bại!");
        }
    }

    [HttpPut]
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
    }

    [HttpDelete]
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
    }

    [HttpGet]
    [Route("/Attendance/GetListByUser")]
    public async Task<IActionResult> GetByUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
            }
            var listAttendance = await Mediator.Send(new GetListAttendanceByApplicationUserIdRequest { Id = id, Page = 1, Size = 20 });
            return Ok(listAttendance);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        }
    }
}
