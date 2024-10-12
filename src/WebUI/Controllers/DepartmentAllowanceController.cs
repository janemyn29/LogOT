using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Department.Commands.CreateDepartment;
using mentor_v1.Application.Department.Commands.DeleteDepartment;
using mentor_v1.Application.Department.Commands.UpdateDepartment;
using mentor_v1.Application.Department.Queries.GetDepartment;
using mentor_v1.Application.Department.Queries.GetDepartmentWithRelativeObjet;
using mentor_v1.Application.DepartmentAllowance.Commands.CreateDepartmentAllowance;
using mentor_v1.Application.DepartmentAllowance.Commands.DeleteDepartmentAllowance;
using mentor_v1.Application.DepartmentAllowance.Commands.UpdateDepartmentAllowance;
using mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowance;
using mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowanceWithRelativeObject;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class DepartmentAllowanceController : ApiControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public DepartmentAllowanceController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    //get list DepartmentAllowance
    [Authorize(Roles = "Manager")]
    [HttpGet]
    [Route("/DepartmentAllowance")]
    public async Task<IActionResult> index(int pg = 1)
    {
        var listDepartmentAllowance = await Mediator.Send(new GetListDepartmentAllowanceRequest { Page = 1, Size = 20 });
        return Ok(listDepartmentAllowance);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [Route("/DepartmentAllowance/Create")]
    public async Task<IActionResult> Create(CreateDepartmentAllowanceCommand model)
    {
        var validator = new CreateDepartmentAllowanceCommandValidator(_context);
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
            var departmentAllowance = await Mediator.Send(new CreateDepartmentAllowanceCommand { DepartmentId = model.DepartmentId, SubsidizeId = model.SubsidizeId });
            return Ok("Tạo thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Tạo thất bại!");
        }
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete]
    [Route("/DepartmentAllowance/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteDepartmentAllowanceCommand { Id = id });
            return Ok("Xóa thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Xóa thất bại!");
        }
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    [Route("/DepartmentAllowance/Update")]
    public async Task<IActionResult> Update(UpdateDepartmentAllowanceCommand model)
    {
        var validator = new UpdateDepartmentAllowanceCommandValidator(_context);
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
            var department = await Mediator.Send(new GetDepartmentAllowanceByIdRequest { Id = model.Id });
            try
            {

                var departmentAllowanceUpdate = await Mediator.Send(new UpdateDepartmentAllowanceCommand { Id = model.Id, DepartmentId = model.DepartmentId, SubsidizeId = model.SubsidizeId });
                return Ok("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy yêu cầu!");

        }
    }
}
