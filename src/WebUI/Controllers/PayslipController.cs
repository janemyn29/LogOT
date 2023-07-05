using DocumentFormat.OpenXml.Bibliography;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
using mentor_v1.Application.Exchange.Queries;
using mentor_v1.Application.InsuranceConfig.Queries;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Application.TaxIncome.Queries;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Services.PayslipServices;

namespace WebUI.Controllers;
[Authorize(Roles = "Manager")]

public class PayslipController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPayslipService _payslipService;

    public PayslipController(UserManager<ApplicationUser> userManager,IPayslipService payslipService)
    {
        _userManager = userManager;
        _payslipService = payslipService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var listUser = await _userManager.Users.Include(c=>c.Position).Where(x => x.WorkStatus == mentor_v1.Domain.Enums.WorkStatus.StillWork).ToListAsync();
        var defaultConfig = await Mediator.Send(new GetDefaultConfigRequest { });
        var tax = await Mediator.Send(new GetListTaxIncomeRequest { });
        var exchange = await Mediator.Send(new GetListExchangeRequest { });
        var regionWage = await Mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = defaultConfig.CompanyRegionType });
        var shiftConfig = await Mediator.Send(new GetListShiftRequest { });
        var insuranceConfig = await Mediator.Send(new GetInsuranceConfigRequest { });
        foreach (var item in listUser)
        {
            //hd dang pending 

            var contract = await Mediator.Send(new GetContractByUserRequest { UserId = item.Id });
            var finalContract = contract.Where(x=>x.Status == EmployeeContractStatus.Pending).FirstOrDefault();
            if(item.UserName == "string")
            {
                var total = await _payslipService.GrossToNetPending(item, defaultConfig, tax, exchange, regionWage, insuranceConfig, DateTime.Parse("2023-07-01"), shiftConfig, finalContract);
            }

            //đã hết hạn trong tháng trước//tính lại lương => ....
        }

        return Ok();
    }
}
