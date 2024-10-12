using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using System.Data;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Department.Queries.GetDepartmentWithRelativeObjet;
using mentor_v1.Application.Level.Queries.GetLevelWithRelativeObject;
using mentor_v1.Application.Positions.Commands.CreatePosition;
using mentor_v1.Application.Positions.Queries.GetPosition;
using mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using mentor_v1.Application.Positions.Commands.UpdatePosition;
using mentor_v1.Application.Positions.Commands.DeletePosition;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.PagingUser;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using mentor_v1.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Controllers;
[Authorize(Roles = "Manager")]

public class PositionController : ApiControllerBase

{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public PositionController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    [HttpGet]
    [Route("/Position")] 
    public async Task<IActionResult> Index(int pg = 1)
    {
        var listPosition = await Mediator.Send(new GetListPositionRequest { Page = pg, Size = 20 });
        return Ok(listPosition);
    }

    [HttpGet]
    [Route("/Position/GetByUser")]
    public async Task<IActionResult> GetByUser(string Username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(Username);
            if(user == null)
            {
                return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
            }
            var position = await Mediator.Send(new GetPositionByIdRequest { Id = user.PositionId });
            PositionModel model = new PositionModel();
            position.ApplicationUsers = null;
            model.Position = position;
            model.User = user;
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        }
    }


    [HttpPost]
    [Route("/Position/Create")]
    public async Task<IActionResult> Create(CreatePositionCommand model)
    {
        var validator = new CreatePositionCommandValidator(_context, _userManager);
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
            var level = await Mediator.Send(new GetLevelByIdRequest { Id = model.LevelId });
            try
            {
                var department = await Mediator.Send(new GetDepartmentByIdRequest { Id = model.DepartmentId });
            }
            catch (Exception)
            {

                return BadRequest("Không tìm thấy phòng ban mà bạn yêu cầu!");
            }
            try
            {

                var position = await Mediator.Send(new CreatePositionCommand { Name = model.Name, DepartmentId = model.DepartmentId, LevelId = model.LevelId });
                return Ok("Tạo vị trí công việc thành công!");
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy trình độ mà bạn yêu cầu!");

        }
    }


    [HttpPut]
    [Route("/Position/Update")]
    public async Task<IActionResult> Update(UpdatePositionCommand model)
    {
        var validator = new UpdatePositionCommandValidator(_context, _userManager);
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
            var level = await Mediator.Send(new GetLevelByIdRequest { Id = model.LevelId });
            try
            {
                var department = await Mediator.Send(new GetDepartmentByIdRequest { Id = model.DepartmentId });
            }
            catch (Exception)
            {

                return BadRequest("Không tìm thấy phòng ban mà bạn yêu cầu!");
            }
            try
            {

                var position = await Mediator.Send(new UpdatePositionCommand { Id = model.Id, Name = model.Name, DepartmentId = model.DepartmentId, LevelId = model.LevelId }) ;
                return Ok("Cập nhật vị trí công việc thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy trình độ mà bạn yêu cầu!");

        }
    }


    [HttpDelete]
    [Route("/Position/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeletePositionCommand { Id = id });
            return Ok("Xóa vị trí công việc thành công!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    [Route("/Position/GetListUser")]
    public async Task<IActionResult> GetListUser(Guid id, int pg= 1)
    {
        try
        {
            Position position = null;
            try
            {
                position = await Mediator.Send(new GetPositionByIdRequest { Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            position.ApplicationUsers = null;
            var listUser =_userManager.Users.Where(x=>x.PositionId == id).OrderBy(x=>x.Fullname).ToList();
            var page = await PagingAppUser<ApplicationUser>
       .CreateAsync(listUser, pg, 20);
            var model = new PaginatedUserModel<Position>();
            model.Defaut = position;
            model.ListUser = page;
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest("Không tìm thấy người dùng bạn yêu cầu");
        } 
    }
}
