using mentor_v1.Application.ApplicationUser.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class DashboardController : ApiControllerBase
{
    [HttpGet]

    public async Task<IActionResult> Index()
    {
        var list = await Mediator.Send(new GetDashboardRequest { });
        return Ok(list);
    }
}
