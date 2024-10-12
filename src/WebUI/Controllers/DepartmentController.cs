using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Degree.Commands.CreateDegree;
using mentor_v1.Application.Degree.Commands.DeleteDegree;
using mentor_v1.Application.Department.Commands.CreateDepartment;
using mentor_v1.Application.Department.Commands.DeleteDepartment;
using mentor_v1.Application.Department.Commands.UpdateDepartment;
using mentor_v1.Application.Department.Queries.GetDepartment;
using mentor_v1.Application.Department.Queries.GetDepartmentWithRelativeObjet;
using mentor_v1.Application.Level.Queries.GetLevel;
using mentor_v1.Application.Level.Queries.GetLevelWithRelativeObject;
using mentor_v1.Application.Positions.Commands.CreatePosition;
using mentor_v1.Application.Positions.Commands.DeletePosition;
using mentor_v1.Application.Positions.Commands.UpdatePosition;
using mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using WebUI.Models;

namespace WebUI.Controllers;

public class DepartmentController : ApiControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public DepartmentController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    //get list department
    [Authorize(Roles ="Manager")]
    [HttpGet]
    [Route("/Department")]
    public async Task<IActionResult> index(int pg = 1)
    {
        var listDepartment = await Mediator.Send(new GetListDepartmentRequest { Page = 1, Size = 20 });
        return Ok(listDepartment);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [Route("/Department/Create")]
    public async Task<IActionResult> Create(CreateDepartmentCommand model)
    {
        var validator = new CreateDepartmentCommandValidator(_context);
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
            var department = await Mediator.Send(new CreateDepartmentCommand { Name = model.Name, Description = model.Description });
            return Ok("Tạo phòng ban thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Tạo phòng ban thất bại!");
        }
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    [Route("/Department/Update")]
    public async Task<IActionResult> Update(UpdateDepartmentCommand model)
    {
        var validator = new UpdateDepartmentCommandValidator(_context);
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
            var department = await Mediator.Send(new GetDepartmentByIdRequest { Id = model.Id });
            try
            {

                var departmentUpdate = await Mediator.Send(new UpdateDepartmentCommand { Id = model.Id, Name = model.Name, Description = model.Description});
                return Ok("Cập nhật phòng ban thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy phòng ban yêu cầu!");

        }
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete]
    [Route("/Department/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteDepartmentCommand { Id = id });
            return Ok("Xóa phòng ban thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Xóa phòng ban thất bại!");
        }
    }

    [Authorize(Roles = "Manager")]
    [HttpGet]
    [Route("/Department/GetByUser")]
    public async Task<IActionResult> GetByUser(string Username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(Username);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
            }
            var position = await Mediator.Send(new GetPositionByIdRequest { Id = user.PositionId });
            PositionModel model = new PositionModel();
            position.ApplicationUsers = null;
            model.Position = position;
            model.User = user;
            var department = await Mediator.Send(new GetDepartmentByIdRequest { Id = model.Position.DepartmentId });
            return Ok(department);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        }
    }

}




