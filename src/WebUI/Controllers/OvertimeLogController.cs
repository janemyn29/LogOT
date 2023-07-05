﻿using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Commands.CreateLevel;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Application.OvertimeLog.Commands.CreateOvertimeLog;
using mentor_v1.Application.OvertimeLog.Commands.DeleteOvertimeLog;
using mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "Manager")]

public class OvertimeLogController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public OvertimeLogController(IApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
    }

    #region [getListForManager]
    [Authorize(Roles = "Manager")]
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

    #region [getListForEmployee]
    [Authorize(Roles = "Employee")]
    [HttpGet]
    public async Task<IActionResult> GetOvertimeLogByEmployeeId()
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);


            var listOTLog = await Mediator.Send(new GetOvertimeLogByUserIdRequest() {id = new Guid(user.Id), Page = 1, Size = 10 });
            return Ok(listOTLog);

        }
        catch (Exception)
        {
            return BadRequest("Không thể lấy danh sách tăng ca");
        }
    }
    #endregion

    #region getOvertimeLogById
    [Authorize(Roles = "Manager, Employee")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOvertimeLogById(Guid id)
    {
        try
        {
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var role = await _userManager.GetRolesAsync(user);
            if (role == null) throw new Exception("user chưa có role");

            var OTLog = Mediator.Send(new GetOvertimeLogByIdRequest() { Id = id, user = user, Role = role.FirstOrDefault() });
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
    [Authorize(Roles = "Manager")]
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
            //lấy user từ username ở header
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            //var count = Mediator.Send(new GetOvertimeLogRequest() { Page = 1, Size = 20 }).Result.TotalCount;
            var oldOT = await Mediator.Send(new GetOvertimeLogByUserIdRequest() { id = new Guid(model.employeeId), Page = 1, Size = 10 });
            foreach (var item in oldOT.Items)
            {
                if (model.Date.ToShortDateString() == item.Date.ToShortDateString())
                {
                    throw new Exception("Nhân viên này đã nhận yêu cầu OT vào ngày: " + model.Date.ToShortDateString());
                }
            }
            var create = await Mediator.Send(new CreateOvertimeLogCommand()
            {
                applicationUserId = user.Id,
                createOvertimeLogViewModel = model
            });

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
    
    #region deleteOvertimeLog
    [Authorize(Roles = "Employee")]
    [HttpPut("{id}")]
    public async Task<IActionResult> DeleteOvertimeLog(Guid id)
    {
        if (id.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");

        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }

        try
        {
            var currentOTLog = await Mediator.Send(new GetOvertimeLogByIdRequest() { Id = id });
            if (currentOTLog.Date > DateTime.Now)
            {
                if (currentOTLog.Status.ToString().ToLower().Equals("request"))
                {
                    var delete = await Mediator.Send(new DeleteOvertimeLogCommand() { Id = id });
                    return Ok("Xóa yêu cầu thành công");
                }
                else
                {
                    return BadRequest(new
                    {
                        Id = id,
                        message = "Không thể xóa, yêu cầu đã được xử lý"
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
    #endregion

    #region approveOvertimeRequest
    [Authorize(Roles = "Employee")]
    [HttpPut]
    public async Task<IActionResult> UpdateStatusOvertimeLogRequest(Guid idOTRequest, string status, string? cancelReason)
    {
        //lấy user từ username ở header
        var username = GetUserName();
        var user = await _userManager.FindByNameAsync(username);

        var listManager = await _userManager.GetUsersInRoleAsync("Manager");


        if (idOTRequest.Equals(Guid.Empty)) return BadRequest("Vui lòng nhập id");
        try
        {
            if (status.ToLower().Equals("approve"))
            {
                var update = await Mediator.Send(new UpdateOvertimeLogRequestStatusCommand()
                {
                    Id = idOTRequest,
                    status = mentor_v1.Domain.Enums.LogStatus.Approved,
                    User = user
                });
                return Ok("Xác nhận yêu cầu thành công");
            }
            else if (status.ToLower().Equals("cancel"))
            {
                var update = await Mediator.Send(new UpdateOvertimeLogRequestStatusCommand()
                {
                    Id = idOTRequest,
                    status = mentor_v1.Domain.Enums.LogStatus.Cancel,
                    cancelReason = cancelReason,
                    User = user
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
                id = idOTRequest,
                message = "Xác nhận trạng thái yêu cầu không thành công"
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
