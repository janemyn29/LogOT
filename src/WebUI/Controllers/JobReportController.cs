
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Security;
using mentor_v1.Application.JobReport.ExportExcelFile;
using mentor_v1.Application.JobReport.Queries.GetJobReport;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles ="Manager")]
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

    #region Get job Id
    [HttpGet("{JobReportId}")]
    public async Task<IActionResult> GetJobReportDetailById(Guid JobReportId)
    {
        try
        {
            var jobID = await Mediator.Send(new GetJobReportByIdRequest { Id = JobReportId });
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

    [HttpGet("{JobReportId}")]
    public async Task<IActionResult> ExportExcelFile(Guid JobReportId) => File(await Mediator.Send(new ExportExcelFile { Id = JobReportId }),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "TemplateExchange.xlsx");
}
