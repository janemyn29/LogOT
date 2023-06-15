using mentor_v1.Application.AnnualWorkingDays.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services.FileManager;

namespace WebUI.Controllers;
public class AnnualWorkingDayController : ApiControllerBase
{
    private readonly IFileService _fileService;

    public AnnualWorkingDayController(IFileService fileService)
    {
        _fileService = fileService;
;

    }
    [HttpPost("CreateEx")]
    //[Authorize(Policy = "Manager")]
    public async Task<IActionResult> CreateEx(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            // Kiểm tra kiểu tệp tin
            if (!IsExcelFile(file))
            {
                return BadRequest("Chỉ cho phép sử dụng file Excel");
            }

            try
            {
                await Mediator.Send(new CreateAnnualWorkingDayEx { file = file});
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest("Thêm thất bại");
    }

    private bool IsExcelFile(IFormFile file)
    {
        // Kiểm tra phần mở rộng của tệp tin có phải là .xls hoặc .xlsx không
        var allowedExtensions = new[] { ".xls", ".xlsx" };
        var fileExtension = Path.GetExtension(file.FileName);
        return allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
    }
}
