﻿using System.IdentityModel.Tokens.Jwt;
using System.Text;
using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Controllers;
public class AccountController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;


    public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IApplicationDbContext context)
    {
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
    }

    [Authorize(Roles = "Manager,Employee")]
    [HttpGet]
    [Route("/Account")]
   public async Task<IActionResult> Index()
    {

        try
        {
            var username = GetUserName();
            var result = await _userManager.FindByNameAsync(username);
            return Ok(result);

        }
        catch (Exception ex)
        {
            return BadRequest("Đã xảy ra lỗi, vui lòng truy cập lại sau!");
        }

    }


    [Authorize(Roles = "Manager,Employee")]
    [HttpPut]
    [Route("/Account/Update")]
    public async Task<IActionResult> Update([FromBody] UserViewModel model)
    {
        model.Username = GetUserName();
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu!");
        }
        var validator = new CreateUseCommandValidator(_context, _userManager);
        var valResult = await validator.ValidateAsync(model);
        try
        {
            var position = await Mediator.Send(new GetPositionByIdRequest { Id = model.PositionId });
        }
        catch (Exception ex)
        {

            return NotFound(ex.Message);
        }
        if (valResult.Errors.Count != 0)
        {

            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            return BadRequest(errors);

        }

        try
        {
            /*                string fileResult = _fileService.SaveImage(model.Image);*/
            var result = await Mediator.Send(new UpdateUserCommand { model = model });
            return Ok("Cập nhật thông tin thành công!");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [NonAction]
    public string GetJwtFromHeader()
    {
        var httpContext = HttpContext.Request.HttpContext;
        if (httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"];
            if (authorizationHeader.ToString().StartsWith("Bearer "))
            {
                return authorizationHeader.ToString().Substring("Bearer ".Length);
            }
        }
        return null;
    }
    [NonAction]

    public string GetUserName() {
        string jwt = GetJwtFromHeader();
        if(jwt == null)
        {
            return null;
        }
        string secretKey = _configuration["JWT:SecrectKey"];

        // Giải mã JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _configuration["JWT:ValidAudience"],
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecrectKey"]))
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, validationParameters, out _);
            return claimsPrincipal.Identity.Name;

        }
        catch (Exception ex)
        {
            // Xử lý lỗi giải mã JWT
            return null;
        }
    }
}
