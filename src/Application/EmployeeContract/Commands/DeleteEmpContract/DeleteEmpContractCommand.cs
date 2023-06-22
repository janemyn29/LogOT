using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.EmployeeContract.Commands.DeleteEmpContract;
public record DeleteEmpContractCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteEmpContractCommandHandler : IRequestHandler<DeleteEmpContractCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteEmpContractCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteEmpContractCommand request, CancellationToken cancellationToken)
    {
        var CurrentCity = _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.IsDeleted == false && x.Id == request.Id).FirstOrDefault();

        if (CurrentCity == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng mà bạn yêu cầu!");
        }
        else if(CurrentCity.Status == Domain.Enums.EmployeeContractStatus.Expeires || CurrentCity.Status == Domain.Enums.EmployeeContractStatus.Pending) {
            throw new NotFoundException("Không thể xóa hợp đồng đã hết hạn hoặc đang còn hạn!");
        }
        CurrentCity.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
