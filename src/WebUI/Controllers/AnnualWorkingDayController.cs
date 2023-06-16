using mentor_v1.Application.AnnualWorkingDays.Commands;
using mentor_v1.Application.AnnualWorkingDays.Commands.Delete;
using mentor_v1.Application.AnnualWorkingDays.Commands.Update;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
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
    }

    [HttpGet]
    [Route("/Annual")]
    //[Authorize(Policy = "Manager")]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var list = await Mediator.Send(new GetListAnnualRequest { Page = pg, Size = 40 });
        return Ok(list);
        
    }


    [HttpPost]
    [Route("/Annual/ImportExcel")]

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


    [HttpPost]
    [Route("/Annual/Create")]

    //[Authorize(Policy = "Manager")]
    public async Task<IActionResult> Create(CreateAnnualCommand model)
    { 
            try
            {
                await Mediator.Send(new CreateAnnualCommand { Day = model.Day , IsHoliday = model.IsHoliday  });
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
    }
    [HttpPut]
    [Route("/Annual/Update")]

    //[Authorize(Policy = "Manager")]
    public async Task<IActionResult> Update(UpdateAnnualCommand model)
    {
        try
        {
            await Mediator.Send(new UpdateAnnualCommand { Id = model.Id,Day = model.Day, IsHoliday = model.IsHoliday });
            return Ok("Cập nhật thành công");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete]
    [Route("/Annual/Delete")]

    //[Authorize(Policy = "Manager")]
    public async Task<IActionResult> Delete([FromForm]Guid id)
    {
        try
        {
            await Mediator.Send(new DeleteAnnualCommand { Id = id });
            return Ok("Xóa thành công");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
