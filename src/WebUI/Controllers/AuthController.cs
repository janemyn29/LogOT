using System.Data;
using System.Text;
using mentor_v1.Application.Auth;
using mentor_v1.Application.Common;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Controllers;
public class AuthController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    private readonly IApplicationDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager, IIdentityService identityService, IApplicationDbContext context)
    {
        _userManager = userManager;
        _identityService = identityService;
        _context = context;
    }


    [HttpPost]
    [Route("/Login")]
    public async Task<IActionResult> Login([FromBody]LoginWithPassword model)
    {
        try
        {
            //var result = await _identityService.AuthenticateAsync(email, password);
            string callbackUrl = "";
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Username);
                if (user == null)
                {
                    return BadRequest("Tài khoản này không tồn tại!");
                }
            }
            if (user.EmailConfirmed == false)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                callbackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });
                var result = await Mediator.Send(new Login { Username = model.Username, Password = model.Password, callbackUrl = callbackUrl });
                if (result==null)
                {
                    return BadRequest("Đăng nhập không thành công!");
                }
                return Ok(result);
            }
            else
            {
                var result = await Mediator.Send(new Login { Username = model.Username, Password = model.Password });
                if (result == null)
                {
                    return BadRequest("Đăng nhập không thành công!");
                }
                return Ok(result);
            }

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet]
    [Route("/ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string? code, string? userId)
    {
        if (userId == null || code == null)
        {
            return BadRequest("Xác nhận Email không thành công! Link xác nhận không chính xác ! Vui lòng sử dụng đúng link được gửi từ TechGenius tới Email của bạn!");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("Xác nhận Email không thành công! Link xác nhận không chính xác! Vui lòng sử dụng đúng link được gửi từ TechGenius tới Email của bạn!");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        string StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        if (result.Succeeded)
        {
            return Ok("Xác nhận Email thành công!Bây giờ bạn có thể đăng nhập vào tài khoản của mình bằng Email hoặc Username vừa xác thực !");
        }
        else
        {
            return BadRequest("Xác nhận Email không thành công! Link xác nhận không chính xác hoặc đã hết hạn! Vui lòng sử dụng đúng link được gửi từ TechGenius tới Email của bạn!");

        }
    }

}
