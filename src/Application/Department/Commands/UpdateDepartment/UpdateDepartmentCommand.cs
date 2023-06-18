using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Department.Commands.UpdateDepartment;

public record UpdateDepartmentCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; set; }

    public string Description { get; set; }

}
public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var CurrentDepartment = await _context.Get<Domain.Entities.Department>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentDepartment == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Department), request.Id);
        }

        CurrentDepartment.Name = request.Name;
        CurrentDepartment.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}