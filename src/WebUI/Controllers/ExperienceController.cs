using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Department.Commands.CreateDepartment;
using mentor_v1.Application.Department.Commands.DeleteDepartment;
using mentor_v1.Application.Department.Commands.UpdateDepartment;
using mentor_v1.Application.Department.Queries.GetDepartment;
using mentor_v1.Application.Department.Queries.GetDepartmentWithRelativeObjet;
using mentor_v1.Application.Experience.Commands.CreateExperience;
using mentor_v1.Application.Experience.Commands.DeleteExperience;
using mentor_v1.Application.Experience.Commands.UpdateExperience;
using mentor_v1.Application.Experience.Queries.GetExperience;
using mentor_v1.Application.Experience.Queries.GetExperienceWithRelativeObject;
using mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

[Authorize(Roles = "Manager")]

public class ExperienceController : ApiControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public ExperienceController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    //get list department
    [HttpGet]
    [Route("/Experience")]
    public async Task<IActionResult> index(int pg = 1)
    {
        var listExperience = await Mediator.Send(new GetListExperienceRequest { Page = 1, Size = 20 });
        return Ok(listExperience);
    }

    [HttpPost]
    [Route("/Experience/Create")]
    public async Task<IActionResult> Create(CreateExperienceCommand model)
    {
        var validator = new CreateExperienceCommandValidator(_context);
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
            var experience = await Mediator.Send(new CreateExperienceCommand
            {
                ApplicationUserId = model.ApplicationUserId,
                NameProject = model.NameProject,
                TeamSize = model.TeamSize,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
                TechStack = model.TechStack
            });
            return Ok("Tạo kinh nghiệm thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Tạo kinh nghiệm thất bại!");
        }
    }

    [HttpPut]
    [Route("/Experience/Update")]
    public async Task<IActionResult> Update(UpdateExperienceCommand model)
    {
        var validator = new UpdateExperienceCommandValidator(_context);
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
            var experience = await Mediator.Send(new GetExperienceByIdRequest { Id = model.Id });
            try
            {

                var experienceUpdate = await Mediator.Send(new UpdateExperienceCommand
                {
                    Id = model.Id,
                    ApplicationUserId = model.ApplicationUserId,
                    NameProject = model.NameProject,
                    TeamSize = model.TeamSize,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Description = model.Description,
                    TechStack = model.TechStack
                });
                return Ok("Cập nhật kinh nghiệm thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy kinh nghiệm yêu cầu!");

        }
    }

    [HttpDelete]
    [Route("/Experience/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteExperienceCommand { Id = id });
            return Ok("Xóa kinh nghiệm thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest("Xóa kinh nghiệm thất bại!");
        }
    }

    [HttpGet]
    [Route("/Experience/GetListByUser")]
    public async Task<IActionResult> GetByUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
            }
            var listExperience = await Mediator.Send(new GetListExperienceByApplicationUserIdRequest { Id = id,Page = 1, Size = 20 });
            return Ok(listExperience);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        }
    }
}
