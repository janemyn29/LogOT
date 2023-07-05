using mentor_v1.Application.Coefficients.Commands;
using mentor_v1.Application.Coefficients.Queries.GetCoefficients;
using mentor_v1.Application.ConfigDays.Commands.UpdateConfigDay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace WebUI.Controllers;

[Authorize(Roles = "Manager")]
public class CoefficientController :ApiControllerBase
{
    [HttpGet]
    [Route("/Coefficient")]
    public async Task< IActionResult> Index()
    {
        var list = await Mediator.Send(new GetListCoefficientRequest { Page = 1, Size = 4 });

        return Ok(list);
    }

    [HttpPost]
    [Route("/Coefficient/Update")]
    public async Task<IActionResult> Update([FromBody] UpdateCoefficientCommand model)
    {
        try
        {
            await Mediator.Send(new UpdateCoefficientCommand { Id = model.Id, AmountCoefficient = model.AmountCoefficient });
            return Ok("Cập nhật cấu hình hệ số thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
    }
}
