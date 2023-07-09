using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;
using mentor_v1.Application.LeaveLog.Commands.DeleteLeaveLog;
using mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;
using mentor_v1.Application.OvertimeLog.Commands.DeleteOvertimeLog;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LeaveLogController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public LeaveLogController(IApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    #region [getListForManager]
    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetLeaveLog(int page)
    {
        try
        {
            var listLeaveLog = await Mediator.Send(new GetLeaveLogRequest() { Page = page, Size = 20 });
            return Ok(listLeaveLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách nghỉ");
        }
    }
    #endregion

    #region [GetLeaveLogFilterByStatus]
    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetLeaveLogFilterByStatus(LogStatus logStatus)
    {
        try
        {
            var listLeaveLog = await Mediator.Send(new GetListLeaveLogNoPG{ });
            var result = listLeaveLog.Where(x=>x.Status.Equals(logStatus.ToString())).ToList();
            return Ok(result);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách nghỉ");
        }
    }
    #endregion

    /*#region [getListForEmployee]
    [Authorize(Roles = "Employee")]
    [HttpGet]
    public async Task<IActionResult> GetListLeaveLogByEmployeeId(int pg)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var listOTLog = await Mediator.Send(new GetListLeaveLogByUserIdRequest() {userId = new Guid(user.Id), Page = pg, Size=20});
            return Ok(listOTLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách nghỉ làm");
        }
    }
    #endregion*/

    #region getLeaveLogById
    [Authorize(Roles = "Manager")]
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
                message = "Không tìm thấy yêu cầu nghỉ phép cần truy vấn"
            });
        }
    }
    #endregion
/*
    #region [create]
    [Authorize(Roles = "Employee")]
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
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            var create = Mediator.Send(new CreateLeaveLogCommand() {user = user, createLeaveLogViewModel = model });
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
    [HttpPut ("{id}")]
    public async Task<IActionResult> DeleteLeaveLog(Guid id)
    {
        if (id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");

        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }

        try
        {
            var currentLeaveLog = await Mediator.Send(new GetLeaveLogByIdRequest() { Id = id });
            if (currentLeaveLog.LeaveDate > DateTime.Now)
            {
                if (currentLeaveLog.Status.ToString().ToLower().Equals("request"))
                {
                    var delete = await Mediator.Send(new DeleteLeaveLogCommand() { Id = id});
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
            else
            {
                return BadRequest(new
                {
                    Id = id,
                    message = "Không thể xóa, ngày yêu cầu đã qua thời gian hiện tại"
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
    #endregion*/

    #region approveLeaveRequest
    [Authorize(Roles = "Manager")]
    [HttpPut ("{id}")]
    public async Task<IActionResult> UpdateStatusLeaveLogRequest(Guid id, string userId, string status, string? cancelReason)
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
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _userManager.GetRolesAsync(user);
            if (role.FirstOrDefault().ToString().ToLower() != "employee")
            {
                throw new Exception("Chỉ có thể thực hiện với employee !");
            }

            if (status.ToLower().Equals("approve"))
            {
                var update = await Mediator.Send(new UpdateLeaveLogRequestStatusCommand() 
                {
                    Id = id, 
                    applicationUserId = userId, 
                    status = mentor_v1.Domain.Enums.LogStatus.Approved 
                });
                return Ok("Xác nhận yêu cầu thành công");
            }
            else if (status.ToLower().Equals("cancel"))
            {
                var update = await Mediator.Send(new UpdateLeaveLogRequestStatusCommand()
                {
                    Id = id,
                    applicationUserId = userId,
                    status = mentor_v1.Domain.Enums.LogStatus.Cancel,
                    cancelReason = cancelReason
                });
                return Ok("Từ chối yêu cầu thành công");
            }
            //return Ok("Xác nhận yêu cầu thành công");
            throw new Exception();
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                id = id,
                message = "Xác nhận trạng thái không thành công : " + e.Message
            });
        }
    }
    #endregion

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
        string secretKey = _configuration["JWT:SecrectKey"];

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
