using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace mentor_v1.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    public readonly IWebHostEnvironment _environment;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
        IWebHostEnvironment environment
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _environment = environment;
    }

    public async Task<ApplicationUser> GetUserAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user;
    }

    public async Task<string> FindByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.Users.FirstAsync(u => u.Email == email);

            return user.Email;
        }
        catch
        {
            return null;

        }
    }

    public async Task<ApplicationUser> FindUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.Users.FirstAsync(u => u.Email == email);

            return user;
        }
        catch
        {
            return null;

        }
    }


    public async Task<ApplicationUser> FindUserByUsernameAsync(string username)
    {
        try
        {
            var user = await _userManager.Users.FirstAsync(u => u.UserName == username);

            return user;
        }
        catch
        {
            return null;

        }
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string username, string email, string password, string fullname, string image, string address, string identityNumber, DateTime birthDay, string BankAccountNumber, string BankAccountName, string BankName, Guid PositionId, GenderType gender, bool IsMaternity, WorkStatus workStatus, string PhoneNumber)

    {
        var user = new ApplicationUser
        {
            UserName = username,
            Email = email,
            Fullname= fullname,
            Image = image,
            Address = address,
            IdentityNumber = identityNumber,
            BirthDay = birthDay,
            BankAccountName= BankAccountName,
            BankName = BankName,
            BankAccountNumber=BankAccountNumber,
            PositionId=PositionId,
            IsMaternity = IsMaternity,
            GenderType = gender,
            WorkStatus = workStatus,
            PhoneNumber = PhoneNumber

        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

   
    public async Task<(Result Result, string UserId)> CreateAllUserAsync(string fullname, string username, string email, string password , string address, DateTime birthday , string phone , string avatar, string avatarurl)

    {
        var user = new ApplicationUser
        {
           
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }


    public async Task<(Result Result, string UserId)> ModifyAllUserAsync(string fullname, string username, string email, string password, string address, DateTime birthday, string phone, string avatar, string avatarurl)

    {
        var user = new ApplicationUser
        {
            
        };

        var result = await _userManager.UpdateAsync(user);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    /* public async Task<string> AuthenticateAsync(string username, string password)
     {

         var user = await _userManager.FindByNameAsync(username);

         if (user == null)
         {
             user = await _userManager.FindByEmailAsync(username);
             if (user == null)
             {
                 throw new KeyNotFoundException($"Không tìm thấy tên đăng nhập hoặc địa chỉ email '{username}'");
             }
         }
         if (user.LockoutEnd != null && user.LockoutEnd.Value > DateTime.Now)
         {
             throw new KeyNotFoundException($"Tài khoản này hiện tại đang bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ");
         }
         if (user.EmailConfirmed == false)
         {
             throw new KeyNotFoundException($"Email của tài khoản này chưa được xác nhận. Vui lòng nhấn quên mật khẩu!");

         }

         //sign in  
         var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
         if (signInResult.Succeeded)
         {
             var roles = await _userManager.GetRolesAsync(user);
             List<Claim> authClaims = new List<Claim>();
             authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
             authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
             foreach (var item in roles)
             {
                 authClaims.Add(new Claim(ClaimTypes.Role, item));
             }

             var authenKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(_configuration["JWT:SecrectKey"]));

             var token = new JwtSecurityToken(
                 issuer: _configuration["JWT:ValidIssuer"],
                 audience: _configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddDays(1),
                 claims: authClaims,
                 signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                 );
             return new JwtSecurityTokenHandler().WriteToken(token);

             //return await _userClaimsPrincipalFactory.CreateAsync(user) ?? throw new InvalidOperationException("Authenticated failed, please contact administrator!");
         }

         throw new InvalidOperationException("Sai mật khẩu. Vui lòng nhập lại!");
     }*/


    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy tên đăng nhập hoặc địa chỉ email '{username}'");
            }
        }
        if (user.LockoutEnd != null && user.LockoutEnd.Value > DateTime.Now)
        {
            throw new KeyNotFoundException($"Tài khoản này hiện tại đang bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ");
        }
        if(user.WorkStatus == WorkStatus.Quit)
        {
            throw new KeyNotFoundException($"Tài khoản này hiện tại đang bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ");
        }
        if (user.EmailConfirmed == false)
        {
            throw new KeyNotFoundException($"Email của tài khoản này chưa được xác nhận. Vui lòng nhấn quên mật khẩu!");

        }

        //sign in  
        var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (signInResult.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> authClaims = new List<Claim>();
            authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            authClaims.Add(new Claim(ClaimTypes.Name, user.UserName));

            authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach (var item in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecrectKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        throw new InvalidOperationException("Sai mật khẩu. Vui lòng nhập lại!");
    }

    public async Task<bool> SendEmailConfirmAsync(string username,string callbackUrl)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy tên đăng nhập hoặc địa chỉ email '{username}'");
            }
        }
        if (user.LockoutEnd != null && user.LockoutEnd.Value > DateTime.Now)
        {
            throw new KeyNotFoundException($"Tài khoản này hiện tại đang bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ");
        }
            SendMail mail = new SendMail();
            var temp = mail.SendEmailNoBccAsync(user.Email, "Email Xác Nhận Từ Công ty TechGenius",
                "<style>\r\n    body {\r\n      font-family: Arial, sans-serif;\r\n      line-height: 1.5;\r\n    }\r\n    .container {\r\n      max-width: 600px;\r\n      margin: 0 auto;\r\n      padding: 20px;\r\n    }\r\n    h1 {\r\n      color: #333;\r\n    }\r\n    p {\r\n      margin-bottom: 20px;\r\n    }\r\n    .button {\r\n      display: inline-block;\r\n      background-color: #007bff;\r\n      color: #fff;\r\n      padding: 10px 20px;\r\n      text-decoration: none;\r\n      border-radius: 5px;\r\n    }\r\n  </style>\r\n  <div class=\"container\">\r\n    <h1>Xác nhận địa chỉ email của công ty TechGenius</h1>\r\n    <p>Xin vui lòng xác nhận rằng đây là địa chỉ email chính thức của công ty bằng cách nhấp vào nút bên dưới:</p>\r\n   " +$" <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Link xác nhân địa chỉ Email</a>" + "    <p>Vui lòng lưu ý rằng đường dẫn xác nhận sẽ chỉ có hiệu lực trong vòng 30 phút. Sau thời gian đó, đường dẫn sẽ hết hiệu lực và bạn sẽ cần yêu cầu xác nhận lại.</p>\r\n    <p>Nếu có bất kỳ thay đổi hoặc sự nhầm lẫn nào liên quan đến địa chỉ email của công ty, xin vui lòng thông báo cho chúng tôi ngay lập tức để chúng tôi có thể cập nhật thông tin đúng cho công ty.</p>\r\n    <p>Xin cảm ơn vì sự hỗ trợ của quý công ty trong việc xác nhận địa chỉ email. Chúng tôi mong muốn tiếp tục hợp tác và cung cấp các dịch vụ công nghệ tốt nhất cho công ty của quý vị.</p>\r\n   "
              );
            if (temp == true)
            {
            return true;
            }
            else
            {
            return false;

            }

    }
}
