using System.Collections.Generic;
using mentor_v1.Application.ConfigDays.Commands.UpdateConfigDay;
using mentor_v1.Application.ConfigDays.Queries.GetConfigDay;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
//[Authorize]
public class ConfigDayController : ApiControllerBase
{
    [HttpGet]
    [Route("/ConfigDay")]
    public async Task<IActionResult> Index()
    {
        var list = await Mediator.Send(new GetListConfigDayRequest { Page = 1, Size = 10 });
        return Ok(list.Items.FirstOrDefault());
    }

    [HttpPost]
    [Route("/ConfigDay/Update")]
    public async Task<IActionResult> Update([FromBody] UpdateConfidDayCommand  config)
    {
        try
        {
            await Mediator.Send(new UpdateConfidDayCommand { Normal = config.Normal, Holiday = config.Holiday, Saturday = config.Saturday, Sunday = config.Sunday });
            return Ok("Cập nhật cấu hình ca làm việc thành công!");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
    }
}
