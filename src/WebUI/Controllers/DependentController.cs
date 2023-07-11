using DocumentFormat.OpenXml.Office2010.Excel;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Dependent.Commands.CreateDependent;
using mentor_v1.Application.Dependent.Commands.DeleteDependentCommand;
using mentor_v1.Application.Dependent.Commands.UpdateDependent;
using mentor_v1.Application.Dependent.Queries;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Packaging.Ionic.Zip;

namespace WebUI.Controllers;

[ApiController]
[Authorize(Roles = "Manager")]

[Route("[controller]/[action]")]
public class DependentController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public DependentController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    #region Get List
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListDependent(int page)
    {
        try
        {
            var result = await Mediator.Send(new GetDependentRequest { Page = page, Size = 20 });
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

    #region Get id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDependentId(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new GetDependentIdRequest { id = id });
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

    #region Get id
    [HttpGet]
    public async Task<IActionResult> GetListDependantByStatus(int pg = 1)
    {
        try
        {
            var result = await Mediator.Send(new GetListDependanceByAcceptance { Page = pg , AcceptanceType = mentor_v1.Domain.Enums.AcceptanceType.Request, Size = 20 });
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


    #region Get id
    [HttpGet]
    public async Task<IActionResult> GetListDependantByUser(string userId, int pg = 1)
    {
        try
        {
            var result = await Mediator.Send(new GetDependantByUserId { Page = pg, userId = userId, Size = 20 });
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

    #region Get id
    [HttpPut]
    public async Task<IActionResult> UpdateAcceptance(Guid id,AcceptanceType acceptanceType)
    {
        try
        {
            var result = await Mediator.Send(new GetDependentIdRequest { id = id });

            Mediator.Send(new UpdateDepartmentAcceptanceCommand { AcceptanceType = acceptanceType, Id = id });
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
    public async Task<IActionResult> CreateDependent(CreateDependentViewModel createDependentViewModel)
    {

        var validator = new CreateDepentdentCommandValidator(_context);
        var valResult = await validator.ValidateAsync(createDependentViewModel);

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
            var create = await Mediator.Send(new CreateDependentCommand
            {
                createDependentViewModel = createDependentViewModel
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
                message = "ApplicationUserId không xuất hiện."
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

    # region Update
    [HttpPut]
    public async Task<IActionResult> Update(UpdateDependentViewModel UpdateDependentViewModel)
    {

        var validator = new UpdateDependentValidator(_context);
        var valResult = await validator.ValidateAsync(UpdateDependentViewModel);

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
            var update = await Mediator.Send(new UpdateDependentCommand
            {
                _updateDependentViewModel = UpdateDependentViewModel
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
    public async Task<IActionResult> DeleteDependent(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteDependentCommand { Id = id });
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
