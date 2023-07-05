
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.ExcelEmployeeQuit.Queries.GetListExcelEmployeeQuit;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ExcelEmployeeQuitController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public ExcelEmployeeQuitController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    #region
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListExcelEmployeeQuit(int page)
    {
        try
        {
            var listAllowance = await Mediator.Send(new GetListExcelEmployeeQuitRequest { Page = page, Size = 10 });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách thành công.",
                Result = listAllowance
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
