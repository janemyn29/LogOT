using mentor_v1.Application.Allowance.Commands.CreateAllowance;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Degree.Commands.CreateDegree;
using mentor_v1.Application.Degree.Commands.DeleteDegree;
using mentor_v1.Application.Degree.Commands.UpdateDegree;
using mentor_v1.Application.Degree.Queries.GetDegree;
using mentor_v1.Application.Degree.Queries.GetDegreeByRelatedObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using WebUI.Models;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DegreeController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public DegreeController(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }


    #region Get List
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListDegree(int page)
    {
        try {
            var result = await Mediator.Send(new GetDegreeRequest { Page = page, Size = 20 });
            return Ok(new {
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


    #region Get List
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListByUserId(int page,string userId)
    {
        try
        {
            var result = await Mediator.Send(new GetListDegreeByUserIdRequets { Page = page, Size = 20,UserId = userId });
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
    public async Task<IActionResult> GetDegreeId(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new GetDegreeIdRequest { id = id });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Lấy dự liệu thành công.",
                result = result
            });
        }
        catch (NotFoundException ex) {
            return NotFound(new {
                status = NotFound().StatusCode,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> CreateDegree(CreateDegreeViewModel createDegreeViewModel)
    {
        var validator = new CreateDegreeCommadValidator(_context);
        var valResult = await validator.ValidateAsync(createDegreeViewModel);

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
            var create = await Mediator.Send(new CreateDegreeCommand
            {
                createDegreeViewModel = createDegreeViewModel
            });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Tạo thành công."
            });
        }
        catch (Exception ex) {
            return BadRequest(new {
                status = BadRequest().StatusCode,
                message = "Tạo thất bại."
            });
        }
    }
    #endregion

    # region Update
    [HttpPut]
    public async Task<IActionResult> Update(UpdateDegreeViewModel updateDegreeViewModel) 
    {

        var validator = new UdpateDegreeValidator(_context);
        var valResult = await validator.ValidateAsync(updateDegreeViewModel);

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
            var update = await Mediator.Send(new UpdateDegreeCommand
            {
                _updateDegreeViewModel = updateDegreeViewModel
            });
            return Ok(new
            {
                status = Ok().StatusCode,
                message = "Cập nhật thành công."
            });

        } catch (NotFoundException ex)
        { 
            return NotFound(new { 
                staus = NotFound().StatusCode,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new {
                status = NotFound().StatusCode,
                message = "Cập nhật thất bại."
            });
        }
    }

    #endregion

    #region Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDegree(Guid id)
    {
        try {
            var result = await Mediator.Send(new DeleteDegreeCommand { Id = id });
            return Ok(new {
                status = Ok().StatusCode,
                message = "Xoá thành công"
            });
        } catch (NotFoundException ex) { 
            return NotFound(new { 
                staus = NotFound().StatusCode,
                message = ex.Message
            });
        }

    }
    #endregion

}
