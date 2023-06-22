using System.Collections.Generic;
using mentor_v1.Application.ConfigDays.Commands.UpdateConfigDay;
using mentor_v1.Application.ConfigDays.Queries.GetConfigDay;
using mentor_v1.Application.DefaultConfig.Commands;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.RegionalMinimumWage.Commands;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Application.TaxIncome.Queries;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
//[Authorize]
public class ConfigDayController : ApiControllerBase
{
    [HttpGet]
    [Route("/Config/ShiftDay")]
    public async Task<IActionResult> Index()
    {
        var list = await Mediator.Send(new GetListConfigDayRequest { Page = 1, Size = 10 });
        return Ok(list.Items.FirstOrDefault());
    }

    [HttpPost]
    [Route("/Config/ShiftDay/Update")]
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

    [HttpGet]
    [Route("/Config/Default")]
    public async Task<IActionResult> ConfigDefault()
    {
        try
        {
            var item = await Mediator.Send(new GetDefaultConfigRequest { });
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpPut]
    [Route("/Config/Default/Update")]
    public async Task<IActionResult> UpdateDefault([FromBody] UpdateDefaultConfigCommand model)
    {
        try
        {
            var item = await Mediator.Send(new UpdateDefaultConfigCommand { CompanyRegionType = model.CompanyRegionType, BaseSalary = model.BaseSalary, PersonalTaxDeduction = model.PersonalTaxDeduction, DependentTaxDeduction = model.DependentTaxDeduction, InsuranceLimit = model.InsuranceLimit });
            return Ok("Cập nhật cấu hình mặc định thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet]
    [Route("/Config/RegionalMinimumWage")]
    public async Task<IActionResult> ConfigWage()
    {
        try
        {
            var item = await Mediator.Send(new GetListWageRequest { });
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [Route("/Config/RegionalMinimumWage/Update")]
    public async Task<IActionResult> UpdateWage([FromBody] UpdateWageCommand model)
    {
        try
        {
            var item = await Mediator.Send(new UpdateWageCommand { Id=model.Id, RegionType= model.RegionType, Amount = model.Amount });
            return Ok("Cập nhật cấu hình lương tối thiểu của vùng thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    [HttpGet]
    [Route("/Config/TaxIncome")]
    public async Task<IActionResult> ConfigTaxIncome()
    {
        try
        {
            var item = await Mediator.Send(new GetListTaxIncomeRequest { });
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [Route("/Config/TaxIncome/Update")]
    public async Task<IActionResult> UpdateTax([FromBody] UpdateWageCommand model)
    {
        try
        {
            var item = await Mediator.Send(new UpdateWageCommand { Id = model.Id, RegionType = model.RegionType, Amount = model.Amount });
            return Ok("Cập nhật cấu hình lương tối thiểu của vùng thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

}
