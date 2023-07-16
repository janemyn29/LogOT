using System.IdentityModel.Tokens.Jwt;
using System.Text;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Note.Queries.GetNotificationByRelativeObject;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Controllers;

[ApiController]
[Route ("[controller]/[action]")]
public class NotificationController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public NotificationController(IApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Employee")]
    public async Task<IActionResult> GetNotificationByUserId(int page = 1)
    {
        try
        {
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var listNoti = await Mediator.Send(new GetListNotificationByUserIdRequest() { userId = user.Id, Page = page , Size = 20 });
            return Ok(listNoti);
        }
        catch (Exception)
        {

            return BadRequest("Không lấy được danh sách thông báo");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Employee")]
    public async Task<IActionResult> IsHaveNotificate()
    {
        try
        {
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var listNoti = await Mediator.Send(new GetIsHaveNotificateRequest() { userId = user.Id});
            return Ok(listNoti);
        }
        catch (Exception)
        {

            return BadRequest("Không lấy được danh sách thông báo");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Employee")]
    public async Task<IActionResult> GetNotificationById(Guid id)
    {
        try
        {
            var username = GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            var listNoti = await Mediator.Send(new GetNotificationById() {  Id= id });
            if (!listNoti.ApplicationUserId.ToLower().Equals(user.Id.ToLower()))
            {
                return BadRequest("Bạn không có quyền truy cập vào thông báo này!");
            }
            return Ok(listNoti);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
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
