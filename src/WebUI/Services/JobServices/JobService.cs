using System.Data;
using System.Threading;
using ClosedXML.Excel;
using Hangfire.Common;
using MediatR;
using mentor_v1.Application.AnnualWorkingDays.Commands;
using mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
using mentor_v1.Application.ExcelContract.Commands.Create;
using mentor_v1.Application.ExcelEmployeeQuit.Commands;
using mentor_v1.Application.Exchange.Queries;
using mentor_v1.Application.InsuranceConfig.Queries;
using mentor_v1.Application.JobReport.Commands.Create;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Application.Payday.Commands;
using mentor_v1.Application.Payday.Queries;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Application.TaxIncome.Queries;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using mentor_v1.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using WebUI.Models;
using WebUI.Services.PayslipServices;

namespace WebUI.Services.JobServices;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly IPayslipService _payslipService;

    public JobService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMediator mediator, IPayslipService payslipService)
    {
        _context = context;
        _userManager = userManager;
        _mediator = mediator;
        _payslipService = payslipService;

    }

    public async Task<int> FillEmptyWorkDay()
    {
        var now = DateTime.Now;
        var workday = await _context.Get<AnnualWorkingDay>().Where(x => x.Day.Date == now.Date && x.IsDeleted == false).FirstOrDefaultAsync();
        if (workday == null)
        {
            await _mediator.Send(new CreateNormalDayCommand { Day = now.Date });
        }
        return 200;
    }

    public async Task<int> NoticeContractExpire()
    {
        var listUser = await _userManager.Users.Include(x => x.EmployeeContracts)
            .Where(x => x.WorkStatus == WorkStatus.StillWork
            && x.EmployeeContracts.Any(c => c.IsDeleted == false
            && c.Status == EmployeeContractStatus.Pending && c.EndDate.Value.Date == DateTime.Now.Date.AddDays(5))).ToListAsync();

        if (listUser == null || listUser.Count <= 0)
        {
            return 404;
        }
        else
        {
            var listManager = await _userManager.GetUsersInRoleAsync("Manager");
            var JobId = await _mediator.Send(new CreateJobReportCommand { Title = "Danh sách hợp đồng sắp hết hạn ngày " + DateTime.Now.Date.AddDays(5).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.Date.ToString("dd/MM/yyyy") + ")", Job = "Thông báo hợp đồng sắp kết thúc", ActionType = ActionType.NoticeExperiodContract, ActionDate = DateTime.Now });
            foreach (var item in listUser)
            {
                var contract = item.EmployeeContracts.Where(c => c.IsDeleted == false
            && c.Status == EmployeeContractStatus.Pending && c.EndDate.Value.Date == DateTime.Now.Date.AddDays(5)).FirstOrDefault();
                var noteId = await _mediator.Send(new CreateNotiCommand
                {
                    ApplicationUserId = item.Id,
                    Title = "Thông báo về việc hợp đồng của bạn sắp hết hạn!",
                    Description = "Hiện hợp đồng " + contract.ContractCode + " sắp hết hạn. Nếu bạn vẫn tiếp tục làm việc sau khi hợp đồng kết thúc thì vui lòng liên hệ với Quản lý để ký thêm hợp đồng mới!"
                });

                var excel = await _mediator.Send(new CreateExcelContractCommand
                {
                    ContractCode = contract.ContractCode,
                    JobReportId = JobId,
                    StartDate = DateTime.Parse(contract.StartDate.ToString()),
                    EndTime = DateTime.Parse(contract.EndDate.ToString()),
                    IdentityNumber = item.IdentityNumber,
                    EmployeeName = item.Fullname,
                    Action = ActionType.NoticeExperiodContract,
                    ActionDate = DateTime.Now,
                    ContractStatus = contract.Status.ToString()
                });
            }
            foreach (var temp in listManager)
            {
                var tempNote = await _mediator.Send(new CreateNotiCommand
                {
                    ApplicationUserId = temp.Id,
                    Title = "Thông báo về việc hợp đồng của " + listUser.Count + " nhân viên sắp hết hạn!",
                    Description = "Hiện hợp đồng của " + listUser.Count
                    + " nhân viên sắp hết hạn. Vui lòng truy cập vào Mục tự động và liên hệ với với các nhân viên đó để thảo luận về hợp đồng!"
                });
            }
            return listUser.Count;

        }
    }

    public async Task<int> NoticeEmptyWorkday()
    {
        var now = DateTime.Now;
        var workday = await _context.Get<AnnualWorkingDay>().Where(x => x.Day.Date == now.Date.AddDays(1)).FirstOrDefaultAsync();
        if (workday == null)
        {
            string title = "Thiếu cấu hình ngày làm việc cho ngày " + now.AddDays(1).ToString("dd/MM/yyyy");
            string des = "Vui lòng bổ sung cấu hình ngày làm việc cho ngày " + now.AddDays(1).ToString("dd/MM/yyyy") + " để việc chấm công và tính lương được thực hiện chính xác nhất!";
            var listManager = await _userManager.GetUsersInRoleAsync("Manager");
            foreach (var item in listManager)
            {
                await _mediator.Send(new CreateNotiCommand { ApplicationUserId = item.Id, Title = title, Description = des });
            }
        }
        return 200;
    }

    public async Task<int> NoticeFillAnnualWorkingDay()
    {
        var nextMonth = DateTime.Now.AddMonths(1);
        string title = "Thông báo thêm bổ sung cấu hình ngày làm việc cho tháng " + nextMonth.ToString("MM/yyyy");
        string des = "Vui lòng bổ sung bổ sung cấu hình ngày làm việc cho tháng " + nextMonth.ToString("MM/yyyy") + " để việc chấm công và tính lương được thực hiện chính xác nhất!";
        var listManager = await _userManager.GetUsersInRoleAsync("Manager");
        foreach (var item in listManager)
        {
            await _mediator.Send(new CreateNotiCommand { ApplicationUserId = item.Id, Title = title, Description = des });
        }
        return 200;

    }

    /*    public async Task<int> NoticeContractExpire()
        {
            var listUser = await _userManager.Users.ToListAsync();
            if(listUser == null || listUser.Count <= 0 ) {
                return 404;
            }
            return listUser.Count;
        }*/

    public async Task<int> ScheduleCheckEndContract()
    {
        
        var listContract = await _context.Get<EmployeeContract>()
            .Include(x => x.ApplicationUser).ThenInclude(x => x.EmployeeContracts)
            .ToListAsync();
        var list = listContract.Where(x => x.IsDeleted == false && x.ContractType == ContractType.FixedTerm && x.Status != EmployeeContractStatus.Expeires && x.EndDate.Value.Date == DateTime.Now.AddDays(-1).Date).ToList();
        if (list == null || list.Count <= 0)
        {
            return 404;
        }
        else
        {
            List<string> tempListUser = new List<string>();
            bool check = false;
            string title = "Kết thúc hợp đồng Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")";
            var jobId = await _mediator.Send(new CreateJobReportCommand { Title = title, ActionDate = DateTime.Now, Job = "Kết thúc hợp đồng", ActionType = ActionType.ExperiodContract });
            foreach (var item in list)
            {
                await _mediator.Send(new UpdateEmpContractStatusCommand { ContractCode = item.ContractCode, Status = mentor_v1.Domain.Enums.EmployeeContractStatus.Expeires });
                await _mediator.Send(new CreateExcelContractCommand
                {
                    JobReportId = jobId,
                    ContractCode = item.ContractCode,
                    StartDate = item.StartDate.Value,
                    EndTime = item.EndDate.Value,
                    EmployeeName = item.ApplicationUser.Fullname,
                    IdentityNumber = item.ApplicationUser.IdentityNumber,
                    ContractStatus = EmployeeContractStatus.Expeires.ToString(),
                    Action = ActionType.ExperiodContract,
                    ActionDate = DateTime.Now,
                });

                if (!item.ApplicationUser.EmployeeContracts.Any(x => x.Status == EmployeeContractStatus.Waiting && x.StartDate.Value.Date == DateTime.Now.Date))
                {
                    check = true;
                    
                    tempListUser.Add(item.ApplicationUser.Id.ToLower());
                    await _mediator.Send(new UpdateUserWorkStatusRequest { id = item.ApplicationUserId });

                }
            }
            Guid nullGuid;
            if (check == true)
            {
                string tempTitle = "Thôi việc nhân viên Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")";
                nullGuid =  _mediator.Send(new CreateJobReportCommand { Title = tempTitle, Job = "Thôi việc Nhân Viên", ActionType = ActionType.EmployeeTermination, ActionDate = DateTime.Now }).Result;
          
                foreach (var item in list)
                {
                    if(tempListUser.Contains(item.ApplicationUser.Id.ToLower()))
                    {
                        var temp = _mediator.Send(new CreateExcelEmployeeQuitCommand
                        {
                            Username = item.ApplicationUser.UserName,
                            FullName = item.ApplicationUser.Fullname,
                            Identity = item.ApplicationUser.IdentityNumber,
                            Email = item.ApplicationUser.Email,
                            JobReportId = nullGuid,
                            Phone = item.ApplicationUser.PhoneNumber,
                            ActionDate = DateTime.Now,
                            ActionType = ActionType.EmployeeTermination,
                            WorkStatus = WorkStatus.Quit

                        }).Result;
                    }
                    
                }
            }

            var manager = await _userManager.GetUsersInRoleAsync("Manager");
            

            foreach (var item in manager)
            {
                if (check)
                {
                    string tempDes = "Hệ thống tự động của TechGenius vừa cập nhật Thôi việc cho nhân viên có hợp đồng hết hạn ngày  " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " và không ký tiếp hợp đồng (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + "). Vui lòng truy cập vào Mục Tự Động để xem thêm!";
                    await _mediator.Send(new CreateNotiCommand { ApplicationUserId = item.Id, Description = tempDes, Title = "Thôi việc cho nhân viên Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")" });
                }

                string des = "Hệ thống tự động của TechGenius vừa cập nhật Kết thúc hợp đồng cho " + listContract.Count() + " hợp đồng hết hạn ngày  " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + "). Vui lòng truy cập vào Mục Tự Động để xem thêm!";
                await _mediator.Send(new CreateNotiCommand { ApplicationUserId = item.Id, Description = des, Title = "Kết thúc hợp đồng Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")" });
            }
            return 200;
        }
    }

    public async Task<int> ScheduleCheckStartContract()
    {

        var listContract = await _context.Get<EmployeeContract>()
            .Include(x => x.ApplicationUser)
            .ToListAsync();
        var tempList = listContract.Where(x=>x.ApplicationUser.WorkStatus == WorkStatus.StillWork && x.StartDate.Value == DateTime.Now.Date && x.Status == EmployeeContractStatus.Waiting).ToList();
        if (tempList == null || tempList.Count <= 0)
        {
            return 404;
        }
        else
        {
            string title = "Bắt đầu hợp đồng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")";
            var jobId = await _mediator.Send(new CreateJobReportCommand { Title = title, ActionDate = DateTime.Now, Job = "Bắt đầu hợp đồng", ActionType = ActionType.StartContract });
            foreach (var item in tempList)
            {
                DateTime date = DateTime.Now;
                if (item.EndDate.Value == null)
                {
                    date = DateTime.Now;
                }
                else
                {
                    date = item.EndDate.Value;
                }
                await _mediator.Send(new UpdateEmpContractStatusCommand { ContractCode = item.ContractCode, Status = mentor_v1.Domain.Enums.EmployeeContractStatus.Pending });
                await _mediator.Send(new CreateExcelContractCommand
                {
                    JobReportId = jobId,
                    ContractCode = item.ContractCode,
                    StartDate = item.StartDate.Value,
                    EndTime = item.EndDate.Value,
                    EmployeeName = item.ApplicationUser.Fullname,
                    IdentityNumber = item.ApplicationUser.IdentityNumber,
                    ContractStatus = EmployeeContractStatus.Pending.ToString(),
                    Action = ActionType.StartContract,
                    ActionDate = DateTime.Now,
                });
            }

            var manager = await _userManager.GetUsersInRoleAsync("Manager");
            foreach (var item in manager)
            {
                string des = "Hệ thống tự động của TechGenius vừa cập nhật Bắt đầu hợp đồng cho " + listContract.Count() + " hợp đồng bắt đầu vào ngày  " + DateTime.Now.ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + "). Vui lòng truy cập vào Mục Tự Động để xem thêm!";
                await _mediator.Send(new CreateNotiCommand { ApplicationUserId = item.Id, Description = des, Title = "Bắt đầu hợp đồng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")" });
            }
            return 200;
        }
    }


    public async Task<int> ScheduleCaculateSalary()
    {

        var now = DateTime.Now;
        //var now = DateTime.Parse("2023-07-01");
        var listUser = await _userManager.Users.Include(c => c.Position).Where(x => x.WorkStatus == mentor_v1.Domain.Enums.WorkStatus.StillWork).ToListAsync();
        var defaultConfig = await _mediator.Send(new GetDefaultConfigRequest { });
        var tax = await _mediator.Send(new GetListTaxIncomeRequest { });
        var exchange = await _mediator.Send(new GetListExchangeRequest { });
        var regionWage = await _mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = defaultConfig.CompanyRegionType });
        var shiftConfig = await _mediator.Send(new GetListShiftRequest { });
        var insuranceConfig = await _mediator.Send(new GetInsuranceConfigRequest { });
        int payday = 1;
        if (now.Day != payday)
        {
            throw new Exception("Tính lương chỉ có thể thực hiện vào ngày 1 hàng tháng!");
        }

        var listPayday = await _mediator.Send(new GetListPaydayRequest { });
        var lastPayday = listPayday.OrderByDescending(x => x.PaymentDay).FirstOrDefault();
        if (lastPayday != null)
        {
            if (now.Date <= lastPayday.PaymentDay.Date)
            {
                throw new Exception("Ngày tính lương không thể trùng với ngày trả lương lần trước hoặc ngày bắt đầu hợp đồng");
            }
        }

        var listManager = await _userManager.GetUsersInRoleAsync("Manager");

        foreach (var item in listUser)
        {
            var Manager = listManager.Where(x => x.Id == item.Id).FirstOrDefault();
            if (Manager == null)
            {
                var contract = await _mediator.Send(new GetContractByUserRequest { UserId = item.Id });
                //hd dang pending 
                try
                {
                    if (contract != null)
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

                    }
                }
                catch (Exception)
                {
                }
            }
            await _mediator.Send(new CreateNotiCommand
            {
                ApplicationUserId = item.Id,
                Title = "Hoàn thành tính lương tháng " + now.AddDays(-1).Month + "/" + now.AddDays(-1).Year,
                Description = "Lương tháng " + now.AddDays(-1).Month + "/" + now.AddDays(-1).Year + " của nhân viên đã được tính xong.Vui lòng truy cập vào bảng lương để xem chi tiết! "
            });

        }
        await _mediator.Send(new CreatePaydayCommand { PaymentDay = now });
        return 200;
    }




}
