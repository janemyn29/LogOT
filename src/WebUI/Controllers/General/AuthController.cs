using System.Data;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire;
using mentor_v1.Application.Auth;
using mentor_v1.Application.Common;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SonPhat.VietNameseLunarCalendar.Astronomy;
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


   /* [HttpGet]
    [Route("/test")]
    public async Task<IActionResult> Test()
    {

        RecurringJob.AddOrUpdate("myrecurringjob",() => Console.WriteLine("Recurring!"),"16 3 * * *",TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        return Ok();
    }*/

    [HttpPost]
    [Route("/Login")]
    public async Task<IActionResult> Login([FromBody] LoginWithPassword model)
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
                //lấy host để redirect về
                var referer = Request.Headers["Referer"].ToString();
                string schema;
                string host;
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                if (Uri.TryCreate(referer, UriKind.Absolute, out var uri))
                {
                    schema = uri.Scheme; // Lấy schema (http hoặc https) của frontend
                    host = uri.Host; // Lấy host của frontend



                    callbackUrl = schema + "://" + host + Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });
                }
                if (callbackUrl.Equals(""))
                {
                    callbackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });
                }
                //kết thúc lấy host để redirect về và tạo link


                //callbackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });
                var result = await Mediator.Send(new Login { Username = model.Username, Password = model.Password, callbackUrl = callbackUrl });
                if (result == null)
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
                CookieOptions options = new CookieOptions
                {
                    IsEssential = true,
                    Expires = null
                };
                Response.Cookies.Append("Auth", result.Token, options);

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

    /*[HttpPost]
    [AllowAnonymous]
    [Route("/ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        // Kiểm tra tính hợp lệ của yêu cầu
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid request");
        }

        // Kiểm tra xác thực người dùng và tạo mã đặt lại mật khẩu (reset token)
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Gửi email chứa liên kết để đặt lại mật khẩu
        var resetLink = Url.Action("ResetPassword", "Account", new { token = resetToken }, Request.Scheme);

        // Gửi email chứa resetLink đến địa chỉ email người dùng
        // Code gửi email ở đâyChú ý rằng trong ví dụ trên, tôi sử dụng `UserManager` để quản lý người dùng và tạo mã đặt lại mật khẩu (reset token). Bạn có thể sử dụng các công cụ và thư viện khác tương tự để thực hiện các chức năng tương tự trong hệ thống của bạn.

        Ngoài ra, bạn cần thực hiện các bước bổ sung như xác thực email và xác thực token khi người dùng nhấp vào liên kết đặt lại mật khẩu. Điều này giúp đảm bảo tính bảo mật của quá trình đặt lại mật khẩu.

Hy vọng rằng ví dụ trên có thể giúp bạn triển khai chức năng reset mật khẩu trong ASP.NET API.*/






}
