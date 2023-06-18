using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Subsidize.Commands.CreateSubsidize;
using mentor_v1.Application.Subsidize.Commands.UpdateSubsidize;
using mentor_v1.Application.Subsidize.Queries.GetSubsidize;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mentor_v1.Application.Subsidize.Queries.GetSubsidizeWithRelativeObject;
using mentor_v1.Application.Department.Commands.DeleteDepartment;
using mentor_v1.Application.Subsidize.Commands.DeleteSubsidize;

namespace WebUI.Controllers;

public class SubsidizeController : ApiControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public SubsidizeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    //get list Subsidize
    [HttpGet]
    [Route("/Subsidize")]
    public async Task<IActionResult> index(int pg = 1)
    {
        var listSubsidize = await Mediator.Send(new GetListSubsidizeRequest { Page = 1, Size = 20 });
        return Ok(listSubsidize);
    }

    [HttpPost]
    [Route("/Subsidize/Create")]
    public async Task<IActionResult> Create(CreateSubsidizeCommand model)
    {
        var validator = new CreateSubsidizeCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);
        if (valResult.Errors.Count != 0)
        {

            List<string> errors = new List<string>();
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage;
                errors.Add(item);
            }
            return BadRequest(errors);

        }
        try
        {
            var subsidize = await Mediator.Send(new CreateSubsidizeCommand { Name = model.Name, Description = model.Description, Amount = model.Amount });
            return Ok("Tạo trợ cấp thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Tạo trợ cấp thất bại!");
        }
    }

    [HttpPut]
    [Route("/Subsidize/Update")]
    public async Task<IActionResult> Update(UpdateSubsidizeCommand model)
    {
        var validator = new UpdateSubsidizeCommandValidator(_context);
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
            var Subsidize = await Mediator.Send(new GetSubsidizeByIdRequest { Id = model.Id });
            try
            {

                var SubsidizeUpdate = await Mediator.Send(new UpdateSubsidizeCommand { Id = model.Id, Name = model.Name, Description = model.Description, Amount = model.Amount });
                return Ok("Cập nhật trợ cấp thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy trợ cấp yêu cầu!");

        }
    }

    [HttpDelete]
    [Route("/Subsidize/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteSubsidizeCommand { Id = id });
            return Ok("Xóa trợ cấp thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Xóa trợ cấp thất bại!");
        }
    }

}