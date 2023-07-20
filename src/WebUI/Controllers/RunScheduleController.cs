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
        //await _jobService.ScheduleCheckEndContract();
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

    [HttpGet]
    [Route("/ScheduleNoticeEmptyWorkDay")]
    public async Task<IActionResult> ScheduleNoticeEmptyWorkDay()
    {
        RecurringJob.RemoveIfExists("NoticeEmptyWorkDay");
        RecurringJob.AddOrUpdate("NoticeEmptyWorkDay", () => _jobService.NoticeEmptyWorkday(), "0 8 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //await _jobService.NoticeContractExpire();
        return Ok();
    }

    [HttpGet]
    [Route("/ScheduleFillEmptyWorkDay")]
    public async Task<IActionResult> ScheduleFillEmptyWorkDay()
    {
        RecurringJob.RemoveIfExists("FillEmptyWorkDay");
        RecurringJob.AddOrUpdate("FillEmptyWorkDay", () => _jobService.FillEmptyWorkDay(), "30 0 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //await _jobService.FillEmptyWorkDay();
        return Ok();
    }

    [HttpGet]
    [Route("/ScheduleNoticeFillWorkDay")]
    public async Task<IActionResult> ScheduleNoticeFillWorkDay()
    {
        RecurringJob.RemoveIfExists("NoticeFillAnnualWorkingDay");
        RecurringJob.AddOrUpdate("NoticeFillAnnualWorkingDay", () => _jobService.NoticeFillAnnualWorkingDay(), "5 8 27 * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //await _jobService.FillEmptyWorkDay();
        return Ok();
    }

    [HttpGet]
    [Route("/ScheduleCaculateSalary")]
    public async Task<IActionResult> ScheduleCaculateSalary()
    {
        RecurringJob.RemoveIfExists("ScheduleCaculateSalary");
        RecurringJob.AddOrUpdate("ScheduleCaculateSalary", () => _jobService.ScheduleCaculateSalary(), "0 1 1 * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //await _jobService.FillEmptyWorkDay();
        return Ok();
    }
}
