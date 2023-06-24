using System.Security.Claims;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    // Task<string> AuthenticateAsync(string Email, string password);
    Task<string> AuthenticateAsync(string Email, string password);
    Task<bool> SendEmailConfirmAsync(string Email,string callbackUrl);

    Task<(Result Result, string UserId)> CreateUserAsync( string username, string email, string password , string fullname , string image, string address, string identityNumber , DateTime birthDay, string BankAccountNumber , string BankAccountName, string BankName,Guid PositionId, GenderType gender, bool IsMaternity ,WorkStatus workStatus,string PhoneNumber);

    Task<(Result Result, string UserId)> CreateAllUserAsync(string fullname, string username, string email, string password, string address, DateTime birthday, string phone, string avatar, string avatarurl);
    Task<(Result Result, string UserId)> ModifyAllUserAsync(string fullname, string username, string email, string password, string address, DateTime birthday, string phone, string avatar, string avatarurl);

    Task<Result> DeleteUserAsync(string userId);

    Task<Domain.Identity.ApplicationUser> GetUserAsync(string userId);

    Task<string> FindByEmailAsync(string email);

    Task<Domain.Identity.ApplicationUser> FindUserByUsernameAsync(string username);

    Task<Domain.Identity.ApplicationUser> FindUserByEmailAsync(string email);
}
