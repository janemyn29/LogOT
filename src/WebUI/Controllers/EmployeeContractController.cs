using mentor_v1.Application.ApplicationUser.Commands.CreateUse;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using WebUI.Services.FileManager;

namespace WebUI.Controllers;
public class EmployeeContractController : ApiControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;

    public EmployeeContractController(IApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService; 

    }
    [Authorize (Roles ="Manager")]
    [Route("/EmployeeContract")]
    [HttpGet]
    public async Task<IActionResult> Index(int pg = 1)
    {
        var listContract = await Mediator.Send(new GetListEmpContractRequest { Page = 1, Size = 20 }); 
        return Ok(listContract);
    }

    [Authorize(Roles = "Manager")]
    [Route("/EmployeeContract")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] EmpContractViewModel model) 
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
            var user = await Mediator.Send(new GetUserByIdRequest { Id = model.ApplicationUserId });
            var filePath = _fileService.SaveImagePdf(model.File);
            var result = await Mediator.Send(new CreateEmployeeContractCommand { EmpContractViewModel = model, File = filePath});
            return Ok();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
        
    }
}
