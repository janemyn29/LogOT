using System.Data;
using ClosedXML.Excel;
using Hangfire.Common;
using MediatR;
using mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
using mentor_v1.Application.ExcelContract.Commands.Create;
using mentor_v1.Application.ExcelEmployeeQuit.Commands;
using mentor_v1.Application.JobReport.Commands.Create;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using mentor_v1.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Services.JobServices;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public JobService(ApplicationDbContext context,UserManager<ApplicationUser> userManager,IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _mediator = mediator;
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
                    Description = "Hiện hợp đồng " +contract.ContractCode + " sắp hết hạn. Nếu bạn vẫn tiếp tục làm việc sau khi hợp đồng kết thúc thì vui lòng liên hệ với Quản lý để ký thêm hợp đồng mới!"
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
                }) ;
            }
            foreach (var temp in listManager)
            {
                var tempNote = await _mediator.Send(new CreateNotiCommand
                {
                    ApplicationUserId = temp.Id,
                    Title = "Thông báo về việc hợp đồng của "+listUser.Count+" nhân viên sắp hết hạn!",
                    Description = "Hiện hợp đồng của "+listUser.Count
                    +" nhân viên sắp hết hạn. Vui lòng truy cập vào Mục tự động và liên hệ với với các nhân viên đó để thảo luận về hợp đồng!"
                });
            }
            return listUser.Count;

        }
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
            .Include(x=>x.ApplicationUser).ThenInclude(x=>x.EmployeeContracts)
            .Where(x=> x.EndDate.Value.Date == DateTime.Now.AddDays(-1).Date
            && x.IsDeleted == false && x.ContractType == mentor_v1.Domain.Enums.ContractType.FixedTerm
            && x.Status != mentor_v1.Domain.Enums.EmployeeContractStatus.Expeires).ToListAsync();
        if(listContract == null|| listContract.Count<=0)
        {
            return 404;
        }
        else
        {
            Guid nullGuid = new Guid("00000000-0000-0000-0000-000000000000");
            bool check = false;
            string title = "Kết thúc hợp đồng Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")";
            var jobId = await _mediator.Send(new CreateJobReportCommand { Title = title, ActionDate = DateTime.Now, Job = "Kết thúc hợp đồng", ActionType = ActionType.ExperiodContract });
            foreach (var item in listContract)
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
                }) ;

                if(!item.ApplicationUser.EmployeeContracts.Any(x=>x.Status == EmployeeContractStatus.Waiting && x.StartDate.Value.Date == DateTime.Now.Date))
                {
                    if (nullGuid.ToString().ToLower().Equals("00000000-0000-0000-0000-000000000000"))
                    {
                        check = true;
                        string tempTitle = "Thôi việc nhân viên Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")";
                        nullGuid = await _mediator.Send(new CreateJobReportCommand { Title = tempTitle, Job = "Thôi việc Nhân Viên", ActionType = ActionType.EmployeeTermination, ActionDate = DateTime.Now });
                    }
                    await _mediator.Send(new UpdateUserWorkStatusRequest { id = item.ApplicationUserId });

                    await _mediator.Send(new CreateExcelEmployeeQuitCommand
                    {
                        Username = item.ApplicationUser.Fullname,
                        FullName = item.ApplicationUser.Fullname,
                        Identity = item.ApplicationUser.IdentityNumber,
                        Email = item.ApplicationUser.Email,
                        JobReportId = nullGuid,
                        ActionDate = DateTime.Now,
                        ActionType = ActionType.EmployeeTermination,
                        WorkStatus = WorkStatus.Quit

                    });


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
            .Where(x => x.StartDate.Value.Date == DateTime.Now.Date
            && x.IsDeleted == false 
            && x.Status == mentor_v1.Domain.Enums.EmployeeContractStatus.Waiting && x.ApplicationUser.WorkStatus == WorkStatus.StillWork).ToListAsync();
        if (listContract == null || listContract.Count <= 0)
        {
            return 404;
        }
        else
        {
            string title = "Bắt đầu hợp đồng Ngày " + DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " (Job:" + DateTime.Now.ToString("dd/MM/yyyy") + ")";
            var jobId = await _mediator.Send(new CreateJobReportCommand { Title = title, ActionDate = DateTime.Now, Job = "Bắt đầu hợp đồng", ActionType = ActionType.StartContract });
            foreach (var item in listContract)
            {
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
}
