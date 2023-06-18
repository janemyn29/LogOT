using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.DepartmentAllowance.Commands.DeleteDepartmentAllowance;

public record DeleteDepartmentAllowanceCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteDepartmentAllowanceCommandHandler : IRequestHandler<DeleteDepartmentAllowanceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentAllowanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteDepartmentAllowanceCommand request, CancellationToken cancellationToken)
    {
        var CurrentDepartmentAllowance = await _context.Get<Domain.Entities.DepartmentAllowance>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentDepartmentAllowance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.DepartmentAllowance), request.Id);
        }
        CurrentDepartmentAllowance.IsDeleted = true;



        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
