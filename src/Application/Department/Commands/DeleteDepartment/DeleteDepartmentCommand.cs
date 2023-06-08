using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Department.Commands.DeleteDepartment;

public record DeleteDepartmentCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var CurrentDepartment = await _context.Get<Domain.Entities.Department>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentDepartment == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Department), request.Id);
        }
        CurrentDepartment.IsDeleted = true;



        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
