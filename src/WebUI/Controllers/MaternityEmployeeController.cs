using DocumentFormat.OpenXml.Spreadsheet;
using mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Dependent.Commands.DeleteDependentCommand;
using mentor_v1.Application.MaternityEmployee.Commands.CreateMaternityEmployee;
using mentor_v1.Application.MaternityEmployee.Commands.DeleteMaternityEmployee;
using mentor_v1.Application.MaternityEmployee.Commands.UpdateMaternityEmployee;
using mentor_v1.Application.MaternityEmployee.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "Manager")]

public class MaternityEmployeeController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public MaternityEmployeeController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    #region Get
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListMaternityEmployee(int page)
    {
        try 
        {
            var result = await Mediator.Send(new GetMaternityEmployeeRequest { Page = page, Size = 20 });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy danh sách thành công.",
                result = result
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

    #region Get By id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaternityEmployeeId(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new GetMaternityEmployeeIdRequest { Id = id });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy dự liệu thành công.",
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

    #region Create
    [HttpPost]
    public async Task<IActionResult> CreateMaternityEmployee(CreateMaternityEmployeeViewModel createMaternityEmployeeView)

    {
        var validator = new CreateMaternityEmployeeValidator();
        var valResult = await validator.ValidateAsync(createMaternityEmployeeView);

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
            var create = await Mediator.Send(new CreateMaternityEmployeeCommand
            {
                createMaternityEmployeeViewModel = createMaternityEmployeeView
            });
            var result = await Mediator.Send(new UpdateMaterityStatus { id = createMaternityEmployeeView.ApplicationUserId, IsMaternity = true });
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
                message = "Không tìm thấy nhân viên bạn yêu cầu!"
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

    #region Update
    [HttpPut]
    public async Task<IActionResult> UpdateMaternityEmployee(UpdateMaternityEmployeeViewModel updateMaternityEmployeeView)
    {
        var validator = new UpdateMaternityEmployeeValidator();
        var valResult = await validator.ValidateAsync(updateMaternityEmployeeView);

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
            var update = await Mediator.Send(new UpdateMaternityEmployeeCommand
            {
                updateMaternityEmployeeView = updateMaternityEmployeeView
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

    #region Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaternityEmployee(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteMaternityEmployeeCommand { Id = id });
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

    }
    #endregion
}
