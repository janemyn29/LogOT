using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace mentor_v1.Application.Auth;

public class Login : IRequest<UserLogin>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? callbackUrl { get; set; }
}

public class LoginHandler : IRequestHandler<Login, UserLogin>
{

    private readonly IIdentityService _identityService;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;
    public LoginHandler(IIdentityService identityService, UserManager<Domain.Identity.ApplicationUser> userManager, IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _identityService = identityService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserLogin> Handle(Login request, CancellationToken cancellationToken)
    {
        if (request.callbackUrl!=null)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(request.Username);
                if(user == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy tên đăng nhập hoặc địa chỉ email '{request.Username}'");

                }
            }
                var result = await _identityService.SendEmailConfirmAsync(request.Username.Trim(),request.callbackUrl);
                throw new Exception("Tài khoản này chưa xác thực Email. Vui lòng kiểm tra Email được vừa gửi đến hoặc liên hệ Phòng nhân sự để được hỗ trợ!");


        }
        else
        {
            var result = await _identityService.AuthenticateAsync(request.Username.Trim(), request.Password.Trim());

            if (result!=null)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(request.Username);
                    if (user == null)
                    {
                        throw new KeyNotFoundException($"Không tìm thấy tên đăng nhập hoặc địa chỉ email '{request.Username}'");
                    }
                }
                var roles = await _userManager.GetRolesAsync(user);
                

                var userModel = new UserLogin();
                userModel.userId = user.Id;
                userModel.Email = user.Email;
                userModel.FullName = user.Fullname;
                userModel.Username = user.UserName;
                userModel.Image = user.Image;
                userModel.listRoles = roles.ToList();
                userModel.Token = result;

                /*var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
                var employeeId = employee.Id;
                response.Cookies.Append("EmployeeId", employeeId.ToString());*/
                return userModel;
            }

            throw new AuthenticationException("Đăng nhập không thành công!");
        }
        
    }
}
