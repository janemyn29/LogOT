using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
public class CreateEmployeeContractCommand : IRequest<Guid>
{
    public string Id { get; set; }
    public string? File { get; set; }
    public EmpContractViewModel EmpContractViewModel { get; set; }
}

public class CreateEmployeeContractCommandHandler : IRequestHandler<CreateEmployeeContractCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateEmployeeContractCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateEmployeeContractCommand request, CancellationToken cancellationToken)
    {
        var city = new Domain.Entities.EmployeeContract()
        {
            ApplicationUserId =request.Id,
            File = request.File,
            StartDate = request.EmpContractViewModel.StartDate,
            EndDate = request.EmpContractViewModel.EndDate,
            Job = request.EmpContractViewModel.Job,
            BasicSalary = request.EmpContractViewModel.BasicSalary,
            Status = request.EmpContractViewModel.Status,
            PercentDeduction = request.EmpContractViewModel.PercentDeduction,
            SalaryType = request.EmpContractViewModel.SalaryType,
            ContractType = request.EmpContractViewModel.ContractType,
            ContractCode = request.EmpContractViewModel.ContractCode,    
        };

        // add new category
        _context.Get<Domain.Entities.EmployeeContract>().Add(city);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        // return the Guid
        return city.Id;
    }
}
