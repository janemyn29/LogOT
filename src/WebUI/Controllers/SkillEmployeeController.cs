
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.SkillEmployee.Commands.CreateSkillEmployee;
using mentor_v1.Application.SkillEmployee.Commands.DeleteSkillEmployeeCommand;
using mentor_v1.Application.SkillEmployee.Commands.UpdateSkillEmployee;
using mentor_v1.Application.SkillEmployee.Commands.UpdateSkillEmployeeCommand;
using mentor_v1.Application.SkillEmployee.Queries.GetSkillEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "Manager")]

public class SkillEmployeeController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public SkillEmployeeController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    #region Get Skill
    [HttpGet("{page}")]
    public async Task<IActionResult> GetSkillEmployee(int page)
    {
        try
        {
            var result = await Mediator.Send(new GetSkillEmployeeRequest { Page = page, Size = 20 });
            return Ok(new
            {
                staus = Ok().StatusCode,
                message = "lấy danh sách thành công.",
                result = result
            });
        } catch (Exception ex)
        {
            return BadRequest(new {
                status = BadRequest().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Get Skill ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSkillEmployeeId(string id)
    {
        try
        {
            var result = await Mediator.Send(new GetSkillEmployeeIdRequest { id = id });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy dữ liệu thành công.",
                result = result
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                status = NotFound().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Create Skill
    [HttpPost]
    public async Task<IActionResult> CreateSkillEmployee(CreateSkillEmployeeCommandViewModel createSkillEmployeeCommandViewModel)
    {
        var validator = new CreateSkillEmployeeCommandValidator();
        var valResult = await validator.ValidateAsync(createSkillEmployeeCommandViewModel);

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
            var create = await Mediator.Send(new CreateSkillEmployeeCommand
            {
                createSkillEmployeeCommandView = createSkillEmployeeCommandViewModel
            });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Tạo thành công."
            });
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = "ApplicationUserId không xuất hiện tạo thất bại."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = "Tạo thất bại."
            });
        }
    }
    #endregion

    #region Update Skill
    [HttpPut]
    public async Task<IActionResult> UpdateSkillEmployee(UpdateSkillEmployeeCommandViewModel updateSkillEmployeeCommandView)
    {
        var validator = new UpdateSkillEmployeeCommandValidator();
        var valResult = await validator.ValidateAsync(updateSkillEmployeeCommandView);

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
            var update = await Mediator.Send(new UpdateSkillEmployeeCommand
            {
                updateSkillEmployeeCommandView = updateSkillEmployeeCommandView
            });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Cập nhật thành công."
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
                status = NotFound().StatusCode,
                message = "Cập nhật thất bại."
            });
        }
    }
    #endregion

    #region Delete Skill
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSkillEmployee(Guid id)
    {
        try
        {
            var item = await Mediator.Send(new DeleteSkillEmployeeCommand { Id = id });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Xoá thành công.",
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                Status = NotFound().StatusCode,
                Message = "Xoá thất bại."
            });
        }
    }
    #endregion
}

