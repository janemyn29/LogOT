using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Allowance.Commands.DeleteAllowance;
public class DeleteAllowanceCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteAllowanceCommandHandler : IRequestHandler<DeleteAllowanceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteAllowanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAllowanceCommand request, CancellationToken cancellationToken)
    {
        var currentAllowance = await _context.Get<Domain.Entities.Allowance>().FindAsync(new object[] { request.Id }, cancellationToken);

        if (currentAllowance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Allowance), request.Id);
        }
        currentAllowance.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
