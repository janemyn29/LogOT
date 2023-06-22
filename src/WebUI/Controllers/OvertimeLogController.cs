using FluentValidation;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Commands.CreateLevel;
using mentor_v1.Application.OvertimeLog.Commands.CreateOvertimeLog;
using mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OvertimeLogController : ApiControllerBase
{
    private readonly IApplicationDbContext _context;

    public OvertimeLogController(IApplicationDbContext context)
    {
        _context = context;
    }

    #region [getList]
    //[Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetOvertimeLog()
    {
        try
        {
            var listOTLog = await Mediator.Send(new GetOvertimeLogRequest() { Page = 1, Size = 20 });
            return Ok(listOTLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách tăng ca");
        }
    }
    #endregion

    #region getOvertimeLogById
    //[Authorize (Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOvertimeLogById(Guid id)
    {
        try
        {
            var OTLog = Mediator.Send(new GetOvertimeLogByIdRequest() { Id = id });
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
    public async Task<IActionResult> CreateOvertimeLog([FromBody] CreateOvertimeLogViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new CreateOvertimeLogCommandValidator(_context);
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
            var create = Mediator.Send(new CreateOvertimeLogCommand() { createOvertimeLogViewModel = model });
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

    #region updateOvertimeLog
    //[Authorize (Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateOvertimeLog(Guid id, UpdateOvertimeLogViewModel model)
    {
        if (id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");

        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new UpdateOvertimeLogCommandValidator(_context);
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
            var currentOTLog = await Mediator.Send(new GetOvertimeLogByIdRequest() { Id = id });
            if (currentOTLog.Status.ToString().Equals("Request"))
            {
                var update = await Mediator.Send(new UpdateOvertimeLogCommand() { Id = id, updateOvertimeLogViewModel = model });
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

    #region approveOvertimeRequest
    //[Authorize (Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateStatusOvertimeLogRequest(Guid id, string status, string? cancelReason)
    {
        /*if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new UpdateOvertimeLogCommandValidator(_context);
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
            if (status.ToLower().Equals("approved"))
            {
                var update = await Mediator.Send(new UpdateOvertimeLogRequestStatusCommand() { Id = id, status = mentor_v1.Domain.Enums.LogStatus.Approved });
                return Ok("Xác nhận yêu cầu thành công");
            } else if (status.ToLower().Equals("cancel"))
            {
                var update = await Mediator.Send(new UpdateOvertimeLogRequestStatusCommand()
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
