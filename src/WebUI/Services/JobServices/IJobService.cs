namespace WebUI.Services.JobServices;

public interface IJobService
{
    Task<int> ScheduleCheckEndContract();
}
