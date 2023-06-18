using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.DepartmentAllowance.Commands.UpdateDepartmentAllowance;

public record UpdateDepartmentAllowanceCommand : IRequest
{
    public Guid Id { get; init; }
    public Guid DepartmentId { get; set; }
    public Guid SubsidizeId { get; set; }

}
public class UpdateDepartmentAllowanceCommandHandler : IRequestHandler<UpdateDepartmentAllowanceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentAllowanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDepartmentAllowanceCommand request, CancellationToken cancellationToken)
    {
        var CurrentDepartmentAllowance = await _context.Get<Domain.Entities.DepartmentAllowance>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentDepartmentAllowance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DepartmentAllowance), request.Id);
        }

        CurrentDepartmentAllowance.DepartmentId = request.DepartmentId;
        CurrentDepartmentAllowance.SubsidizeId = request.SubsidizeId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
