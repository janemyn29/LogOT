﻿
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.JobReport.Queries.GetJobReport;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class JobReportController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

   
    public JobReportController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }


    #region Get List Job Report
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListJobReport(int page)
    {
        try
        {
            var listJob = await Mediator.Send(new GetListJobReportRequest { Page = page, Size = 10 });
            return Ok(new { 
                Status = Ok().StatusCode,
                Message = "Lấy danh sách thành công",
                Result = listJob
            });
        } catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region GetAllowanceId
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobReportId(Guid id)
    {
        try
        {
            var jobID = await Mediator.Send(new GetJobReportByIdRequest { Id = id });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Lấy dữ liệu thành công.",
                Result = jobID
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                Status = NotFound().StatusCode,
                Message = ex.Message
            });
        }
    }
    #endregion

}
