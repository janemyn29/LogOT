using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
public record UpdateEmpContractCommand : IRequest
{
    public string ContractCode { get; set; }
    public string File { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Job { get; set; }
    public double? BasicSalary { get; set; }
    public EmployeeContractStatus Status { get; set; }
    public double? PercentDeduction { get; set; }
    public SalaryType SalaryType { get; set; }
    public ContractType ContractType { get; set; }
    public Guid Id { get; set; }
    public bool isPersonalTaxDeduction { get; set; }
    public InsuranceType InsuranceType { get; set; }
    public double? InsuranceAmount { get; set; }


}

public class UpdateEmpContractCommandHandler : IRequestHandler<UpdateEmpContractCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmpContractCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateEmpContractCommand request, CancellationToken cancellationToken)
    {
        var CurrentEmpContract = _context.Get<Domain.Entities.EmployeeContract>().Where(x=>x.Id == request.Id && x.IsDeleted ==false ).FirstOrDefault();

        if (CurrentEmpContract == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng bạn yêu cầu!");
        }
        var existContract = _context.Get<Domain.Entities.EmployeeContract>().Where(x=>x.ContractCode.Trim().Equals(request.ContractCode.Trim())&& x.Id !=request.Id && x.IsDeleted==false).FirstOrDefault();
        if(existContract != null) {
            throw new NotFoundException("Mã hợp đồng này đã được sử dụng cho 1 hợp đồng khác!");
        }
        CurrentEmpContract.ContractCode = request.ContractCode;
        CurrentEmpContract.File = request.File;
        CurrentEmpContract.StartDate = request.StartDate;
        CurrentEmpContract.EndDate=request.EndDate;
        CurrentEmpContract.Job= request.Job;
        CurrentEmpContract.BasicSalary  = request.BasicSalary;
        CurrentEmpContract.Status = request.Status;
        CurrentEmpContract.PercentDeduction = request.PercentDeduction;
        CurrentEmpContract.SalaryType = request.SalaryType;
        CurrentEmpContract.ContractType= request.ContractType;
        CurrentEmpContract.isPersonalTaxDeduction = request.isPersonalTaxDeduction;
        CurrentEmpContract.InsuranceType = request.InsuranceType;
        CurrentEmpContract.InsuranceAmount = request.InsuranceAmount;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
