using mentor_v1.Application.Allowance.Commands.DeleteAllowance;
using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.DeleteEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
using mentor_v1.Application.RegionalMinimumWage.Queries;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using WebUI.Models;
using WebUI.Services.FileManager;
using WebUI.Services.Format;

namespace WebUI.Controllers;
[Route("[controller]/[action]")]
public class EmployeeContractController : ApiControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFormatMoney _format;


    public EmployeeContractController(IApplicationDbContext context, IFileService fileService, UserManager<ApplicationUser> userManager, IFormatMoney format)
    {
        _context = context;
        _fileService = fileService;
        _userManager = userManager;
        _format = format;
    }
    /*    [Authorize (Roles ="Manager")]*/
    [HttpGet]
    public async Task<IActionResult> GetList(int pg = 1)
    {
        var listContract = await Mediator.Send(new GetListEmpContractRequest { Page = 1, Size = 20 });
        return Ok(listContract);
    }

    /*    [Authorize (Roles ="Manager")]*/
    [HttpGet]
    public async Task<IActionResult> GetByContractCode(string code)
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


    /*    [Authorize (Roles ="Manager")]*/
    [HttpGet]
    public async Task<IActionResult> GetListByEmployee(string username, int pg = 1)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);

            var list = await Mediator.Send(new GetEmpContractByEmpRequest { Username = username, page = pg, size = 20 });
/*            foreach (var item in list.Items)
            {
                item.ApplicationUser = null;
            }*/
            DefaultModel<PaginatedList<EmpContractViewModel>> repository = new DefaultModel<PaginatedList<EmpContractViewModel>>();
            repository.User = user;
            repository.ListItem = list;
            return Ok(repository);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


   // [Authorize(Roles = "Manager")]
    /*    [Route("/EmployeeContract/Create")]*/
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeContractCommand model)
    {
        //check validation:
        var validator = new CreateEmpContractValidator(_context);
        var valResult = await validator.ValidateAsync(model);
        List<string> errors = new List<string>();
        if (valResult.Errors.Count != 0)
        {
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }
            
        }
        if(model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount <= 0)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác yêu cầu phải cung cấp mức đóng bảo hiểm!";
            errors.Add(item);
        }
        try
        {
            var config = await Mediator.Send(new GetDefaultConfigRequest { });
            var region = await Mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = config.CompanyRegionType});
            if (model.BasicSalary < region.Amount)
            {
                string amount = _format.Format(region.Amount);
                var item = "Lương cơ bản không được thấp hơn mức lương tối thiểu của vùng ("+region.RegionType.ToString()+ ": " +amount+ " VND)";
                errors.Add(item);
            }
        }
        catch (Exception)
        {
            return BadRequest("Đã xảy ra lỗi vui lòng kiểm tra lại các cấu hình về lương và vùng!");
        }
        
        if (errors!=null && errors.Count > 0)
        {
            return BadRequest(errors);
        }

        //kết thúc vheck validator
        try
        {
            
            /*            var filePath = await _fileService.UploadFile(model.File);*/
            var result = await Mediator.Send(new CreateEmployeeContractCommand {
                Username = model.Username,
                ContractCode = model.ContractCode,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                BasicSalary = model.BasicSalary,
                File = model.File,
                Job = model.Job, 
                ContractType = model.ContractType, 
                PercentDeduction = model.PercentDeduction,
                SalaryType = model.SalaryType, 
                Status = model.Status ,
                isPersonalTaxDeduction = model.isPersonalTaxDeduction,
                InsuranceAmount = model.InsuranceAmount,
                InsuranceType = model.InsuranceType,
            });
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
        //check validation:
        var validator = new UpdateEmpContractCommandValidator(_context);
        var valResult = await validator.ValidateAsync(model);
        List<string> errors = new List<string>();
        if (valResult.Errors.Count != 0)
        {
            foreach (var error in valResult.Errors)
            {
                var item = error.ErrorMessage; errors.Add(item);
            }

        }
        if (model.InsuranceType == InsuranceType.BaseOnOtherAmount && model.InsuranceAmount <= 0)
        {
            var item = "Với hình thức đóng bảo hiểm dựa trên số khác yêu cầu phải cung cấp mức đóng bảo hiểm!";
            errors.Add(item);
        }
        try
        {
            var config = await Mediator.Send(new GetDefaultConfigRequest { });
            var region = await Mediator.Send(new GetRegionalWageByRegionTypeNoVm { RegionType = config.CompanyRegionType });
            if (model.BasicSalary < region.Amount)
            {
                string amount = _format.Format(region.Amount);
                var item = "Lương cơ bản không được thấp hơn mức lương tối thiểu của vùng (" + region.RegionType.ToString() + ": " + amount + " VND)";
                errors.Add(item);
            }
        }
        catch (Exception)
        {
            return BadRequest("Đã xảy ra lỗi vui lòng kiểm tra lại các cấu hình về lương và vùng!");
        }
        if (errors != null && errors.Count > 0)
        {
            return BadRequest(errors);
        }
        //kết thúc vheck validator

        try
        {
            ;
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
                 InsuranceAmount= model.InsuranceAmount,
                 InsuranceType= model.InsuranceType,
                 isPersonalTaxDeduction = model.isPersonalTaxDeduction,
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
