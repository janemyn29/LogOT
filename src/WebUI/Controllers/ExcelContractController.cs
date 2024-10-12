
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.ExcelContract.Queries.GetListExcelContacts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ExcelContractController : ApiControllerBase
{

    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public ExcelContractController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    #region Get List Excel Contracts
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListExcelContract(int page, Guid JobReportId)
    {
        try
        {
            var listAllowance = await Mediator.Send(new GetListExcelContractsRequest { Page = page, Size = 10, JobReportId = JobReportId });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách thành công.",
                Result = listAllowance
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

        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion
}
