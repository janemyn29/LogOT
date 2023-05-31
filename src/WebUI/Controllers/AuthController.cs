using mentor_v1.Application.Auth;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
               /* string callbackUrl = Request.Scheme + "://" + Request.Host+ Url.Action("ConfirmEmail", "Auth", new {email= email, })*/
                var user = await Mediator.Send(new Login { Username = email, Password = password });
                if (string.IsNullOrEmpty(user))
                {
                    return BadRequest("Đăng nhập không thành công!");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }

   
}
