using System.Diagnostics.Contracts;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
using mentor_v1.Application.Exchange.Queries;
using mentor_v1.Application.InsuranceConfig.Queries;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Application.Payday.Commands;
using mentor_v1.Application.Payday.Queries;
using mentor_v1.Application.Payslip.Queries;
using mentor_v1.Application.Payslip.Queries.GetList;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Application.TaxIncome.Queries;
using mentor_v1.Domain.Entities;
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

    public PayslipController(UserManager<ApplicationUser> userManager, IPayslipService payslipService)
    {
        _userManager = userManager;
        _payslipService = payslipService;
    }
    [HttpPost]
    [Route("/Payslip/Create")]
    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now;
        //var now = DateTime.Parse("2023-07-01");
        var listUser = await _userManager.Users.Include(c => c.Position).Where(x => x.WorkStatus == mentor_v1.Domain.Enums.WorkStatus.StillWork).ToListAsync();
        var defaultConfig = await Mediator.Send(new GetDefaultConfigRequest { });
        var tax = await Mediator.Send(new GetListTaxIncomeRequest { });
        var exchange = await Mediator.Send(new GetListExchangeRequest { });
        var regionWage = await Mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = defaultConfig.CompanyRegionType });
        var shiftConfig = await Mediator.Send(new GetListShiftRequest { });
        var insuranceConfig = await Mediator.Send(new GetInsuranceConfigRequest { });
        int payday = 1;
        if (now.Day != payday)
        {
            return BadRequest("Tính lương chỉ có thể thực hiện vào ngày 1 hàng tháng!");
        }

        var listPayday = await Mediator.Send(new GetListPaydayRequest { });
        var lastPayday = listPayday.OrderByDescending(x => x.PaymentDay).FirstOrDefault();
        if (lastPayday != null)
        {
            if (now.Date <= lastPayday.PaymentDay.Date)
            {
                return BadRequest("Ngày tính lương không thể trùng với ngày trả lương lần trước hoặc ngày bắt đầu hợp đồng");
            }
        }

        var listManager = await _userManager.GetUsersInRoleAsync("Manager");
        string title = "Hoàn thành tính lương tháng " + now.AddDays(-1).Month + "/" + now.AddDays(-1).Year;
        string des = "Lương tháng " + now.AddDays(-1).Month + "/" + now.AddDays(-1).Year + " của nhân viên đã được tính xong.Vui lòng truy cập vào bảng lương để xem chi tiết! ";

        foreach (var item in listUser)
        {
            var Manager = listManager.Where(x => x.Id == item.Id).FirstOrDefault();
            if (Manager == null)
            {
                var contract = await Mediator.Send(new GetContractByUserRequest { UserId = item.Id });
                //hd dang pending 
                try
                {
                    if (contract != null)
                    {
                        try
                        {
                            var finalContract = contract.Where(x => x.Status == EmployeeContractStatus.Pending).FirstOrDefault();
                        if (finalContract != null)
                        {
                            var total = await _payslipService.GrossToNetPending(item, defaultConfig, tax, exchange, regionWage, insuranceConfig, now, shiftConfig, finalContract);
                        }

                        //đã hết hạn trong tháng trước//tính lại lương => ....
                        var ExpriedContract = contract.Where(x => x.Status == EmployeeContractStatus.Expeires
                        && x.EndDate.Value.Month == now.AddMonths(-1).Month && x.EndDate.Value.Year == now.AddMonths(-1).Year
                        && x.EndDate.Value.Day <= now.AddDays(-1).Day).FirstOrDefault();
                        if (ExpriedContract != null)
                        {
                            var total = await _payslipService.GrossToNetExperid(item, defaultConfig, tax, exchange, regionWage, insuranceConfig, now, shiftConfig, ExpriedContract);
                        }
                        
                            await Mediator.Send(new CreateNotiCommand
                            {
                                ApplicationUserId = item.Id,
                                Title = title,
                                Description = des
                            });
                        }
                        catch (Exception )
                        {
                        }
                        

                    }
                }
                catch (Exception)
                {
                }
            }
            

        }
        await Mediator.Send(new CreatePaydayCommand { PaymentDay = now });
        return Ok("Đã hoàn thành tính lương cho nhân viên!");
    }

    [HttpGet]
    [Route("/Payslip/GetListPayslip")]

    public async Task<IActionResult> GetListPayslip(int pg=1)
    {
        var list = await Mediator.Send(new GetListPayslipRequest { Page = pg, Size = 30 });
        return Ok(list);
    }

    [HttpGet]
    [Route("/Payslip/GetListPayslipByUserOrMonthOrBoth")]

    public async Task<IActionResult> GetListPayslipByUserOrMonthOrBoth(string? userId, int? month, int? year)
    {
        var list = await Mediator.Send(new GetListPayslipNoPg { });
        List<PaySlip> final = new List<PaySlip>();
        if (userId != null && month != null && year != null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) && x.ToTime.Month == month && x.ToTime.Year == year).ToList();

        }else if(userId != null && month != null && year == null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) && x.ToTime.Month == month).ToList();

        }else if(userId != null && month == null && year != null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) &&  x.ToTime.Year == year).ToList();
        }else if(userId == null && month != null && year != null)
        {
            final = list.Where(x => x.ToTime.Month == month && x.ToTime.Year == year).ToList();
        }else if(userId != null && month == null && year == null)
        {
            final = list.Where(x => x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(userId.ToLower()) ).ToList();
        }else if(userId == null && month == null && year != null)
        {
            final = list.Where(x =>  x.ToTime.Year == year).ToList();
        }else if(userId == null && month != null && month == null)
        {
            final = list.Where(x => x.ToTime.Month == month).ToList();
        }
        else
        {
        }
        return Ok(final);
    }


    [HttpGet]
    [Route("/Payslip/GetDetailPayslip")]

    public async Task<IActionResult> GetDetailPayslip(Guid id)
    {
        var list = await Mediator.Send(new GetPaySlipRequets {  Id = id });
        return Ok(list);
    }
}
