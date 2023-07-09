﻿using System.Data;
using System.Text;
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

    [HttpPost]
    [Route("/ResetPassword")]
    public async Task<IActionResult> ResetPassword(string userId, string code, string newPassword, string confirmPassword)
    {
        try
        {
            if (userId == null || code == null || newPassword == null || confirmPassword == null)
            {
                return BadRequest("Vui lòng điền đẩy đủ thông tin được yêu cầu !");
            }
            if (!newPassword.Equals(confirmPassword))
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

            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            if (result.Succeeded)
            {
                // Mật khẩu đã được đặt lại thành công
                return Ok("Mật khẩu đã được đặt lại thành công!");
            }
            else
            {
                // Đặt lại mật khẩu không thành công
                return BadRequest("Không thể đặt lại mật khẩu.");
            }
        }
        catch (Exception e)
        {

            return BadRequest("Đã xảy ra lỗi trong quá trình: " + e.Message);
        }

    }

    [HttpGet]
    [Route("/ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            if (email == null) throw new ArgumentNullException("email");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Không tìm thấy địa chỉ emal");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            // Gửi email chứa liên kết để đặt lại mật khẩu
            string callbackUrl =/* schema + "://" + host +*/ Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, Request.Scheme);

            return Ok(callbackUrl);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest("lỗi: " + e.Message);
        }
        catch (Exception e)
        {

            return BadRequest("Xác nhận email không thành công: " + e.Message);
        }
    }
}