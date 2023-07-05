using mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
using mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;

namespace WebUI.Services.ContractServices;

public interface IContracService
{
     Task<List<string>> CheckValidatorCreate(CreateEmployeeContractCommand model);
    Task<List<string>> CheckValidatorCreateEmployee(CreateEmployeeContractCommand model);
    Task<List<string>> CheckValidatorUpdate(UpdateEmpContractCommand model);

}
