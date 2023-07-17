using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using DocumentFormat.OpenXml.CustomXmlSchemaReferences;
using DocumentFormat.OpenXml.EMMA;
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
using Microsoft.Extensions.Hosting;
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
                Response.Cookies.Append("AuthHangfire", result.Token, options);

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

    [HttpPost]
    [Route("/ResetPassword")]
    public async Task<IActionResult> ResetPassword(string userId, string code,[FromBody] ResetPassModel model)
    {
        try
        {
            if (userId == null || code == null )
            {
                return BadRequest("Không thể đặt lại mật khẩu. Vui lòng sử dụng link đã được gửi tới trong mail của bạn!");
            }
            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                return BadRequest("Mật khẩu với và mật khẩu xác nhận không khớp !");
            }

            // Kiểm tra xác thực người dùng và tạo mã đặt lại mật khẩu (reset token)
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Không tìm thấy tài khoản !");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            
            // Gửi email chứa liên kết để đặt lại mật khẩu
            //var callbackUrl = Url.Action("ResetPassword", "Auth", new { token = code }, Request.Scheme);

            var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);

            if (result.Succeeded)
            {
                // Mật khẩu đã được đặt lại thành công
                return Ok("Mật khẩu đã được đặt lại thành công!");
            }
            else
            {
                // Đặt lại mật khẩu không thành công
                return BadRequest("Không thể đặt lại mật khẩu. Vui lòng sử dụng link đã được gửi tới trong mail của bạn");
            }
        }
        catch (Exception e)
        {

            return BadRequest("Đã xảy ra lỗi trong quá trình: " + e.Message);
        }

    }

    [HttpPost]
    [Route("/ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Không tìm thấy địa chỉ emal");
            }
            //lấy host để redirect về
            var referer = Request.Headers["Reference"].ToString();
            string schema;
            string host;

            string callbackUrl = "";

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            if (Uri.TryCreate(referer, UriKind.Absolute, out var uri))
            {
                schema = uri.Scheme; // Lấy schema (http hoặc https) của frontend
                host = uri.Host; // Lấy host của frontend



                callbackUrl = schema + "://" + host + Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, Request.Scheme);
            }
            if (callbackUrl.Equals(""))
            {
                callbackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });
            }

            string body = "<style>\r\n    body {\r\n        font-family: Arial, sans-serif;\r\n        line-height: 1.5;\r\n        \r\n    }\r\n\r\n    .container {\r\n        max-width: 600px;\r\n         margin: 0 auto;\r\n        padding: 20px;\r\n       \r\n    }\r\n\r\n    h1 {\r\n        color: #333;\r\n        \r\n    }\r\n\r\n    p {\r\n         margin-bottom: 20px;\r\n       \r\n    }\r\n\r\n     .button {\r\n        display: inline-block;\r\n        background-color: #007bff;\r\n        color: #fff;\r\n         padding: 10px 20px;\r\n        text-decoration: none;\r\n         border-radius: 5px;\r\n        \r\n    }\r\n\r\n</style>\r\n <div class=\"container\">\r\n    <h1>Xác nhận yêu cầu đổi mật khẩu truy cập vào Website của công ty TechGenius</h1>\r\n    <p>Bạn đã yêu cầu đổi mật khẩu. Vui lòng nhấp vào liên kết bên dưới để xác nhận yêu cầu:</p> " + $"<a  href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Link đổi mật khẩu</a>" + " <p>Vui lòng lưu ý rằng\r\n        đường dẫn xác nhận sẽ chỉ có hiệu lực trong vòng 30 phút. Sau thời gian đó, đường dẫn sẽ hết hiệu lực và bạn sẽ\r\n        cần yêu cầu xác nhận lại.</p>\\r\\n <p>Nếu có bất kỳ thay đổi hoặc sự nhầm lẫn nào liên quan đến địa chỉ email của\r\n        công ty, xin vui lòng thông báo cho chúng tôi ngay lập tức để chúng tôi có thể cập nhật thông tin đúng cho công\r\n        ty.</p>";
            SendMail mail = new SendMail();
            var temp = mail.SendEmailNoBccAsync(user.Email, "Email đổi mật khẩu Từ Công ty TechGenius", body);

            return Ok("Thông báo về việc gửi link đổi mật khẩu đã được gửi thành công đến địa chỉ email của bạn. Vui lòng kiểm tra hộp thư đến (inbox) của bạn để tìm thư chứa link đổi mật khẩu.");
        }
        catch (Exception e)
        {

            return BadRequest("Xác nhận email không thành công: " + e.Message);
        }
    }
}
