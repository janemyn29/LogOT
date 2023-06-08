using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
public class CreateEmployeeContractCommand : IRequest<Guid>
{
    public string ApplicationUserId { get; set; }
    public string ContractCode { get; set; }

    public string? File { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Job { get; set; }
    public double? BasicSalary { get; set; }
    public EmployeeContractStatus Status { get; set; }
    public double? PercentDeduction { get; set; }
    public SalaryType SalaryType { get; set; }
    public ContractType ContractType { get; set; }
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
            ApplicationUserId = request.ApplicationUserId,
            File = request.File,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Job = request.Job,
            BasicSalary = request.BasicSalary,
            Status = request.Status,
            PercentDeduction = request.PercentDeduction,
            SalaryType = request.SalaryType,
            ContractType = request.ContractType,
            ContractCode = request.ContractCode,    
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
