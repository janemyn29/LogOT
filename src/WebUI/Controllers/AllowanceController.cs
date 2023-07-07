using MediatR;
using mentor_v1.Application.Allowance.Commands.CreateAllowance;
using mentor_v1.Application.Allowance.Commands.DeleteAllowance;
using mentor_v1.Application.Allowance.Commands.UpdateAllowance;
using mentor_v1.Application.Allowance.Queries.GetAllowance;
using mentor_v1.Application.ApplicationAllowance.Commands.UpdateAllowance;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AllowanceController : ApiControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;
    public AllowanceController(IIdentityService identityService, IApplicationDbContext context = null)
    {
        _identityService = identityService;
        _context = context;
    }

    #region Get List Allowance
    [Authorize(Roles = "Manager")]
    [HttpGet]
    [Route("/Allowance/GetAll")]
    public async Task<IActionResult> GetAllListAllowance(int page = 1)
    {
        try
        {
            var listAllowance = await Mediator.Send(new GetAllowanceRequest { Page = page, Size = 10 });
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

    #region Get List Allowance
    [Authorize(Roles = "Manager")]
    [HttpGet("{page}")]
    public async Task<IActionResult> GetListAllowance(int page)
    {
        try
        {
            var listAllowance = await Mediator.Send(new GetAllowanceRequest { Page = page, Size = 10 });
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

    #region GetAllowanceId
    [Authorize(Roles = "Manager,Employee")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllowanceId(Guid id)
    {
        try
        {
            var allowanceId = await Mediator.Send(new GetAllowanceIdRequest { Id = id });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Lấy dữ liệu thành công.",
                Result = allowanceId
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
    }
    #endregion

    #region Create Allowance
    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> CreateAllowance(CreateAllowanceViewModel createAllowanceViewModel)
    {
        var validator = new CreateAllowanceCommandValidator(_context);
        var valResult = await validator.ValidateAsync(createAllowanceViewModel);

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
            var create = await Mediator.Send(new CreateAllowanceCommand
            {
                Name = createAllowanceViewModel.Name,
                AllowanceType = createAllowanceViewModel.AllowanceType,
                Amount = createAllowanceViewModel.Amount,
                Eligibility_Criteria = createAllowanceViewModel.Eligibility_Criteria,
                Requirements = createAllowanceViewModel.Requirements
            });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Tạo thành công."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Status = BadRequest().StatusCode,
                Message = "Tạo thất bại."
            });
        }
    }
    #endregion

    #region Update Allowance
    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> UpdateAllowance(UpdateAllowanceViewModel updateAllowanceViewModel)
    {
        var validator = new UpdateAllowanceCommandValidator(_context);
        var valResult = await validator.ValidateAsync(updateAllowanceViewModel);

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
            var update = await Mediator.Send(new UpdateAllowanceCommand
            {
                updateAllowanceView = updateAllowanceViewModel
            });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Cập nhật thành công."
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
                Status = BadRequest().StatusCode,
                Message = "Cập nhật thất bại."
            });
        }
    }
    #endregion

    #region Delete Allowance
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAllowance(Guid id)
    {
        try
        {
            var item = await Mediator.Send(new DeleteAllowanceCommand { Id = id });
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
                Message = ex.Message
            });
        }

        catch (Exception ex){
            return BadRequest(new
            {
                status = BadRequest().StatusCode,
                message = "Xoá thất bại bời vì id của bảng này đang được sử dụng ở bảng AllowanceEmployee."
            });
        }
    }
    #endregion

    
}
