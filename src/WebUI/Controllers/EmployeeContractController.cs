using mentor_v1.Application.Allowance.Commands.DeleteAllowance;
using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.DeleteEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using WebUI.Services.FileManager;

namespace WebUI.Controllers;
[Route("[controller]/[action]")]
public class EmployeeContractController : ApiControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeContractController(IApplicationDbContext context, IFileService fileService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _fileService = fileService;
        _userManager = userManager;
    }
    /*    [Authorize (Roles ="Manager")]*/
    [HttpGet]
    public async Task<IActionResult> GetList(int pg = 1)
    {
        var listContract = await Mediator.Send(new GetListEmpContractRequest { Page = 1, Size = 20 });
        return Ok(listContract);
    }

    /*    [Authorize (Roles ="Manager")]*/
    [HttpGet()]
    public async Task<IActionResult> GetById(string code)
    {
        try
        {
            var Contract = await Mediator.Send(new GetEmpContractByCodeRequest { code = code });
            return Ok(Contract);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [Authorize(Roles = "Manager")]
    /*    [Route("/EmployeeContract/Create")]*/
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmpContractViewModel model)
    {
        var validator = new CreateEmpContractValidator(_context);
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
            var user = await _userManager.FindByNameAsync(model.Username);
            /*            var filePath = await _fileService.UploadFile(model.File);*/
            var result = await Mediator.Send(new CreateEmployeeContractCommand { EmpContractViewModel = model, Id = user.Id });
            return Ok("Thêm hợp đồng thành công!");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }


/*    [Authorize(Roles = "Manager")]*/
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEmpContractCommand model)
    {
        var validator = new UpdateEmpContractCommandValidator(_context);
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
        {;
            /*            var filePath = await _fileService.UploadFile(model.File);*/
            var result = await Mediator.Send(new UpdateEmpContractCommand {
                Id = model.Id,
                 ContractCode = model.ContractCode,
                 ContractType = model.ContractType,
                 File= model.File,
                 StartDate = model.StartDate,
                 EndDate = model.EndDate,
                 Job= model.Job,
                 PercentDeduction= model.PercentDeduction,
                 BasicSalary= model.BasicSalary,
                 SalaryType= model.SalaryType,
                 Status= model.Status,
            });
            return Ok("Cập nhật hợp đồng thành công!");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("{id}")]
/*    [Route("/EmployeeContract/Delete")]*/
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var item = await Mediator.Send(new DeleteEmpContractCommand { Id = id });
            if (item)
            {
                return Ok("Xóa hợp đồng thành công!");
            }
            return BadRequest("Đã xảy ra lỗi khi xóa hợp đồng!");
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /*
        [Authorize(Roles = "Manager")]
        [Route("/EmployeeContract/DowloadEmpContract")]
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string FileName)
        {
            var result = await _fileService.DownloadFile(FileName);
            return File(result.Item1, result.Item2, result.Item2);
        }*/
}
