using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class EmployeeContractController : ApiControllerBase
{
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

        return Ok();
    }
}
