using System.Security.Claims;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<ClaimsPrincipal> AuthenticateAsync(string Email, string password);

    Task<(Result Result, string UserId)> CreateUserAsync(string fullname, string username, string email, string password,string phone);
     Task<(Result Result, string UserId)> CreateAllUserAsync(string fullname, string username, string email, string password, string address, DateTime birthday, string phone, string avatar, string avatarurl);
    Task<(Result Result, string UserId)> ModifyAllUserAsync(string fullname, string username, string email, string password, string address, DateTime birthday, string phone, string avatar, string avatarurl);

    Task<Result> DeleteUserAsync(string userId);

    Task<ApplicationUser> GetUserAsync(string userId);

    Task<string> FindByEmailAsync(string email);

    Task<ApplicationUser> FindUserByUsernameAsync(string username);

    Task<ApplicationUser> FindUserByEmailAsync(string email);
}
