using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using mentor_v1.Application.Level.Queries.GetLevel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using mentor_v1.Application.Level.Commands.CreateLevel;
using mentor_v1.Application.Common.Interfaces;
using FluentValidation;
using mentor_v1.Application.Level.Commands.UpdateLevel;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LevelController : ApiControllerBase
{

    private readonly IApplicationDbContext _context;

    public LevelController(IApplicationDbContext context)
    {
        _context = context;
    }

    //[Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetLevel()
    {
        var listLevel = await Mediator.Send(new GetLevelRequest { Page = 1, Size = 20 });
        return Ok(listLevel);
    }

    //[Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateLevel([FromForm] LevelViewModel model)
    {
      /*  if (!ModelState.IsValid)
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
*/
        try
        {
            var create = await Mediator.Send(new CreateLevelCommand() { levelViewModel = model });
            return Ok(new
            {
                id = create,
                message = "Khởi tạo thành công"
            });
        }
        catch (Exception)
        {

            return BadRequest("Khởi tạo thất bại");
        }
    }

    //[Authorize ("Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, LevelViewModel model)
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
            var update = await Mediator.Send(new UpdateLevelCommand() {Id = id , LevelViewModel = model});
            return Ok("Cập nhật thành công");
        }
        catch (Exception)
        {

            return BadRequest("Cập nhật không thành công");
        }
    }

    //[Authorize ("Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (Guid id)
    {
        try
        {
            return Ok("Xóa thành công");
        }
        catch (Exception)
        {

            return BadRequest("Xóa không thành công");
        }
    }
}
