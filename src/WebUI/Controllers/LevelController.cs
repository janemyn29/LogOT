using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Commands.CreateLevel;
using mentor_v1.Application.Level.Commands.DeleteLevel;
using mentor_v1.Application.Level.Commands.UpdateLevel;
using mentor_v1.Application.Level.Queries.GetLevel;
using mentor_v1.Application.Level.Queries.GetLevelWithRelativeObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "Manager")]

public class LevelController : ApiControllerBase
{

    private readonly IApplicationDbContext _context;

    public LevelController(IApplicationDbContext context)
    {
        _context = context;
    }

    #region getListLevel
    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetLevel(int pg)
    {
        var listLevel = await Mediator.Send(new GetLevelRequest { Page = pg, Size = 20 });
        return Ok(listLevel);
    }
    #endregion

    #region getLevelById
    [Authorize(Roles = "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLevelById(Guid id)
    {
        try
        {
            var level = await Mediator.Send(new GetLevelByIdRequest() {Id = id });
            return Ok(level);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy cấp độ theo id yêu cầu");
        }
    }
    #endregion

    #region createLevel
    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateLevel([FromBody] LevelViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new CreateLevelCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);

        if (valResult.Errors.Count != 0)
        {
            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            return BadRequest(errors);
        }

        try
        {
            var create = await Mediator.Send(new CreateLevelCommand() { levelViewModel = model });
            return Ok(new
            {
                id = create,
                message = "Khởi tạo thành công"
            });
        }
        catch (Exception e)
        {

            return BadRequest("Khởi tạo thất bại: " + e.Message );
        }
    }
    #endregion

    #region updateLevel
    [Authorize(Roles = "Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LevelViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Vui lòng điền đầy đủ các thông tin được yêu cầu");
        }
        var validator = new UpdateLevelCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);

        if (valResult.Errors.Count != 0)
        {
            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            return BadRequest(errors);
        }
        try
        {
            var update = await Mediator.Send(new UpdateLevelCommand() {Id = id , LevelViewModel = model});
            return Ok("Cập nhật thành công");
        }
        catch (Exception)
        {

            return BadRequest("Cập nhật không thành công");
        }
    }
    #endregion

    #region deleteLevel
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteLevelCommand { Id = id });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Xoá thành công"
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                staus = NotFound().StatusCode,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = "Xóa không thành công! Cấp độ hiện tại đang có liên quan dữ liệu đến 1 số vị trí! "
            });
        }
    }
    #endregion
}
