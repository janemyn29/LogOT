using System.Collections.Generic;
using mentor_v1.Application.AnnualWorkingDays.Commands.Update;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
using mentor_v1.Application.ConfigDays.Commands.UpdateConfigDay;
using mentor_v1.Application.ConfigDays.Queries.GetConfigDay;
using mentor_v1.Application.DefaultConfig.Commands;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.Exchange.Commands.UpdateExchange;
using mentor_v1.Application.Exchange.ExportExcelFileCommands;
using mentor_v1.Application.Exchange.Queries;
using mentor_v1.Application.RegionalMinimumWage.Commands;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Application.TaxIncome.Commands.CreateTaxIncome;
using mentor_v1.Application.TaxIncome.ExportExcelFileTaxIncomeCommands;
using mentor_v1.Application.TaxIncome.Queries;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize(Roles = "Manager")]
public class ConfigDayController : ApiControllerBase
{
    [HttpGet]
    [Route("/Config/WorkDay")]
    public async Task<IActionResult> Index()
    {
        var list = await Mediator.Send(new GetListConfigDayRequest { Page = 1, Size = 10 });
        return Ok(list.Items.FirstOrDefault());
    }

    [HttpPost]
    [Route("/Config/WorkDay/Update")]
    public async Task<IActionResult> Update([FromBody] UpdateConfidDayCommand  config)
    {
        try
        {
            //nếu cập nhật thì xóa hết các annual sau ngày sau ngày sửa. thêm thông báo thêm lại annual.
            await Mediator.Send(new UpdateConfidDayCommand { Normal = config.Normal, Holiday = config.Holiday, Saturday = config.Saturday, Sunday = config.Sunday });

            //đổi lại annual 
            var listAnnual = await Mediator.Send(new GetListAnnualFromCurrent { });
            if(listAnnual != null && listAnnual.Count>0) {
                foreach (var item in listAnnual)
                {
                    bool check = false;
                    if(item.TypeDate == mentor_v1.Domain.Enums.TypeDate.Holiday)
                    {
                        check = true;
                    }
                    await Mediator.Send(new UpdateAnnualCommand() { Day = item.Day, Id = item.Id, IsHoliday = check});
                }
            }
            
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
    [HttpGet]
    [Route("/Config/Exchange")]
    public async Task<IActionResult> ConfigExchange()
    {
        try
        {
            var item = await Mediator.Send(new GetListExchangeRequest { });
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    /* [HttpPut]
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

     }*/

    #region Update TaxIncome

    [HttpPut]
    [Route("/Config/UpdateTaxIncome")]
    public async Task<IActionResult> UpdateTaxIncome(IFormFile file)
    {
        // Kiểm tra kiểu tệp tin
        if (!IsExcelFile(file))
        {
            return BadRequest("Chỉ cho phép sử dụng file Excel");
        }

        try
        {
            await Mediator.Send(new UpdateTaxIncomeCommand { file = file });

            return Ok(new
            {
                status = Ok().StatusCode,
                message = "cập nhật TaxIncome thành công."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                staus = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Update Exchange
    [HttpPut]
    [Route("/Config/UpdateExchange")]
    public async Task<IActionResult> UpdateExchange(IFormFile file)
    {
        // Kiểm tra kiểu tệp tin
        if (!IsExcelFile(file))
        {
            return BadRequest("Chỉ cho phép sử dụng file Excel");
        }

        try
        {
            await Mediator.Send(new UpdateExchangeCommand { file = file });

            return Ok(new
            {
                status = Ok().StatusCode,
                message = "cập nhật TaxIncome thành công."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                staus = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Export Excel File

    [HttpGet]
    [Route("/Config/ExportExcelFileExchange")]
    public async Task<IActionResult> ExportExcelFileExchange() => File(await Mediator.Send(new ExportExcelFileExchange { }),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "TemplateExchange.xlsx");

    [HttpGet]
    [Route("/Config/ExportExcelFileTaxIncome")]
    public async Task<IActionResult> ExportExcelFileTaxIncome() => File(await Mediator.Send(new ExportExcelFileTaxIncome { }),
           "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
           "TemplateTaxIncome.xlsx");

    #endregion

    #region Check Format is File Excel
    private bool IsExcelFile(IFormFile file)
    {
        // Kiểm tra phần mở rộng của tệp tin có phải là .xls hoặc .xlsx không
        var allowedExtensions = new[] { ".xls", ".xlsx" };
        var fileExtension = Path.GetExtension(file.FileName);
        return allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
    }
    #endregion

}
