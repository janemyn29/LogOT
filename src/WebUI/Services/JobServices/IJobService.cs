using MediatR;

namespace WebUI.Services.JobServices;

public interface IJobService
{
    Task<int> ScheduleCheckEndContract();
    Task<int> ScheduleCheckStartContract();
    Task<int> NoticeContractExpire();

}
