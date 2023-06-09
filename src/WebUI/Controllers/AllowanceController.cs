using MediatR;
using mentor_v1.Application.Allowance.Commands.CreateAllowance;
using mentor_v1.Application.Allowance.Commands.DeleteAllowance;
using mentor_v1.Application.Allowance.Commands.UpdateAllowance;
using mentor_v1.Application.Allowance.Queries.GetAllowance;
using mentor_v1.Application.ApplicationAllowance.Commands.UpdateAllowance;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
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
    [HttpGet]
    public async Task<IActionResult> GetListAllowance()
    {
        try
        {
            var listAllowance = await Mediator.Send(new GetAllowanceRequest { Page = 1, Size = 10 });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Get All Succeed.",
                Result = listAllowance
            });
        }
        catch (Exception ex)
        {
            return NotFound(new
            {
                Status = NotFound().StatusCode,
                Message = "Cannot Found This List."
            });
        }
    }
    #endregion

    #region GetAllowanceId
    [HttpGet]
    public async Task<IActionResult> GetAllowanceId(Guid id)
    {
        try
        {
            var allowanceId = await Mediator.Send(new GetAllowanceIdRequest { Id = id });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Get Succeed.",
                Result = allowanceId
            });
        }
        catch (Exception ex)
        {
            return NotFound(new
            {
                Status = NotFound().StatusCode,
                Message = "Not Found Item."
            });
        }
    }
    #endregion

    #region Create Allowance
    [HttpPost]
    public async Task<IActionResult> CreateAllowance([FromForm]CreateAllowanceViewModel createAllowanceViewModel)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest("Please fill out all required information.");
        }

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
                Message = "Create Succeed."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Status = BadRequest().StatusCode,
                Message = "Create Faild."
            });
        }
    }
    #endregion

    #region Delete Allowance
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAllowance(Guid id)
    {
        try
        {
            var item = await Mediator.Send(new DeleteAllowanceCommand { Id = id });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Delete Succeed.",
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                Status = NotFound().StatusCode,
                Message = "Delete Fail."
            });
        }
    }
    #endregion

    #region Update Allowance
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm]UpdateAllowanceViewModel updateAllowanceViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please fill out all required information.");
        }

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
                Id = id,
                Name = updateAllowanceViewModel.Name,
                AllowanceType = updateAllowanceViewModel.AllowanceType,
                Amount = updateAllowanceViewModel.Amount,
                Eligibility_Criteria = updateAllowanceViewModel.Eligibility_Criteria,
                Requirements = updateAllowanceViewModel.Requirements,
            });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Update Succeed."
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
                Message = "Update Faild."
            });
        }
    }
    #endregion


}
