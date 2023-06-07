using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using mentor_v1.Application.Level.Queries.GetLevel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class LevelController : ApiControllerBase
{
    //[Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetLevel()
    {
        var listLevel = await Mediator.Send(new GetLevelRequest { Page = 1, Size = 20 });
        return Ok(listLevel);
    }
}
