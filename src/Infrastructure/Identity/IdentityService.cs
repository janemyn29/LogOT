using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
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

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
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

    public async Task<(Result Result, string UserId)> CreateUserAsync(string username, string email, string password, string fullname, string image, string address, string identityNumber, DateTime birthDay, string BankAccountNumber, string BankAccountName, string BankName)

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


    public async Task<ClaimsPrincipal> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        var a = user;
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
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var principal = await _signInManager.CreateUserPrincipalAsync(user);
            //var token = new JwtSecurityToken(issuer: _configuration["JWT:ValidIssuer"]
            //    , audience: _configuration["JWT:ValidAudience"],
            //    claims: principal.Claims,
            //    expires: DateTime.Now.AddHours(2),
            //    signingCredentials: creds);

            //return new JwtSecurityTokenHandler().WriteToken(token);

            return await _userClaimsPrincipalFactory.CreateAsync(user) ?? throw new InvalidOperationException("Authenticated failed, please contact administrator!");
        }

        throw new InvalidOperationException("Sai mật khẩu. Vui lòng nhập lại!");
    }
}
