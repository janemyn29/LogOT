using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
public record UpdateEmpContractStatusCommand : IRequest
{
    public string ContractCode { get; set; }
    public EmployeeContractStatus Status { get; set; }

}

public class UpdateEmpContractStatusCommandHandler : IRequestHandler<UpdateEmpContractStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmpContractStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateEmpContractStatusCommand request, CancellationToken cancellationToken)
    {
        var CurrentEmpContract = _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.ContractCode == request.ContractCode && x.IsDeleted == false).FirstOrDefault();

        if (CurrentEmpContract == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng bạn yêu cầu!");
        }else if(CurrentEmpContract.Status == EmployeeContractStatus.Expeires )
        {
            throw new NotFoundException("Không thể cập nhật đối với các hợp đồng đã hết hạn!");
        }
        CurrentEmpContract.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
