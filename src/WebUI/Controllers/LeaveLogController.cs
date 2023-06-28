using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;
using mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LeaveLogController : ApiControllerBase
{
    private readonly IApplicationDbContext _context;

    public LeaveLogController(IApplicationDbContext context)
    {
        _context = context;
    }

    #region [getList]
    //[Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetLeaveLog()
    {
        try
        {
            var listLeaveLog = await Mediator.Send(new GetLeaveLogRequest() { Page = 1, Size = 20 });
            return Ok(listLeaveLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách nghỉ");
        }
    }
    #endregion

    #region getLeaveLogById
    //[Authorize (Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveLogById(Guid id)
    {
        try
        {
            var OTLog = Mediator.Send(new GetLeaveLogByIdRequest() { Id = id });
            return Ok(OTLog);
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                Id = id,
                message = "Không tìm thấy id cần truy vấn"
            });
        }
    }
    #endregion

    #region [create]
    //[Authorize (Roles = "Manager")]
    [HttpPost]
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
            var create = Mediator.Send(new CreateLeaveLogCommand() { createLeaveLogViewModel = model });
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

    #region updateLeaveLog
    //[Authorize (Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateLeaveLog(Guid id, UpdateLeaveLogViewModel model)
    {
        if (id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");

        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new UpdateLeaveLogCommandValidator(_context);
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
            var currentOTLog = await Mediator.Send(new GetLeaveLogByIdRequest() { Id = id });
            if (currentOTLog.Status.ToString().ToLower().Equals("request"))
            {
                var update = await Mediator.Send(new UpdateLeaveLogCommand() { Id = id, updateLeaveLogViewModel = model });
                return Ok("Cập nhật yêu cầu thành công");
            }
            else
            {
                return BadRequest(new
                {
                    Id = id,
                    message = "Không thể cập nhật, yêu cầu đã được xử lý"
                });
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

    #region approveLeaveRequest
    //[Authorize (Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateStatusLeaveLogRequest(Guid id, string status, string? cancelReason)
    {
        /*if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new UpdateLeaveLogCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);

        if (valResult.Errors.Count != 0)
        {
            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            return BadRequest(errors);
        }*/

        if (id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");
        try
        {
            if (status.ToLower().Equals("approve"))
            {
                var update = await Mediator.Send(new UpdateLeaveLogRequestStatusCommand() { Id = id, status = mentor_v1.Domain.Enums.LogStatus.Approved });
                return Ok("Xác nhận yêu cầu thành công");
            }
            else if (status.ToLower().Equals("cancel"))
            {
                var update = await Mediator.Send(new UpdateLeaveLogRequestStatusCommand()
                {
                    Id = id,
                    status = mentor_v1.Domain.Enums.LogStatus.Cancel,
                    cancelReason = cancelReason
                });
                return Ok("Từ chối yêu cầu thành công");
            }
            //return Ok("Xác nhận yêu cầu thành công");
            throw new Exception();
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                id = id,
                message = "Xác nhận trạng thái không thành công"
            });
        }
    }
    #endregion
}
