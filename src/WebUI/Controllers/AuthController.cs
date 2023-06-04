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
    public async Task<IActionResult> Login(string email, string password)
    {
        
        if(User.Identity.IsAuthenticated)
        {
            var response = new ObjectResult("Bạn đã đăng nhập. Vui lòng không đăng nhập thêm!")
            {
                StatusCode = 429
            };
            return await Task.FromResult<IActionResult>(response);
        }
        else
        {
            try
            {
                //var result = await _identityService.AuthenticateAsync(email, password);
                string callbackUrl = "";
               var user = await _userManager.FindByNameAsync(email);
                if(user == null)
                {
                    user = await _userManager.FindByEmailAsync(email);
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
                    var result = await Mediator.Send(new Login { Username = email, Password = password, callbackUrl = callbackUrl });
                    if (result)
                    {
                        return BadRequest("Đăng nhập không thành công!");
                    }
                    return Ok(result);
                }
                else
                {
                    var result = await Mediator.Send(new Login { Username = email, Password = password });
                    /*var roles = await _userManager.GetRolesAsync(user);
                    var userModel = new UserLogin();
                    userModel.Email = user.Email;
                    userModel.FullName = user.Fullname;
                    userModel.Username = user.UserName;
                    userModel.Image = user.Image;
                    userModel.listRoles = roles.ToList();
                    *//*CookieOptions option = new CookieOptions {
                        Expires = DateTime.Now.AddHours(1)
                }
                    ;*//*
                    string infor = JsonConvert.SerializeObject(userModel);
                    HttpContext.Session.SetString("UserInfor", infor);*/

                    if (!result)
                    {
                        return BadRequest("Đăng nhập không thành công!");
                    }
                    return Ok("Đăng nhập thành công!");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }

    [HttpGet]
    [Route("/ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail( string? code,string? userId)
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
