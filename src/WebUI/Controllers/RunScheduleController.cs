using Hangfire;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services.JobServices;

namespace WebUI.Controllers;
public class RunScheduleController : ApiControllerBase
{
    private readonly IJobService _jobService;
    public RunScheduleController(IJobService jobService)
    {
        _jobService = jobService;
    }


    [HttpGet]
    [Route("/ScheduleCheckStartContract")]
    public async Task<IActionResult> ScheduleCheckStartContract()
    {
        RecurringJob.RemoveIfExists("StartContract");
        RecurringJob.AddOrUpdate("StartContract", () => _jobService.ScheduleCheckStartContract(), "30 6 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        return Ok();
    }


    [HttpGet]
    [Route("/ScheduleCheckEndContract")]
    public async Task<IActionResult> ScheduleCheckEndContract()
    {
        RecurringJob.RemoveIfExists("ExperiedContract");
        RecurringJob.AddOrUpdate("ExperiedContract", () => _jobService.ScheduleCheckEndContract(), "0 6 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        return Ok();
    }

    [HttpGet]
    [Route("/ScheduleNoticeOContractExpired")]
    public async Task<IActionResult> ScheduleNoticeOContractExpired()
    {
        RecurringJob.RemoveIfExists("NoticeContractExpire");
        RecurringJob.AddOrUpdate("NoticeContractExpire", () => _jobService.NoticeContractExpire(), "0 7 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //await _jobService.NoticeContractExpire();
        return Ok();
    }
}
