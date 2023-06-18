using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Subsidize.Commands.DeleteSubsidize;

public record DeleteSubsidizeCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteSubsidizeCommandHandler : IRequestHandler<DeleteSubsidizeCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteSubsidizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteSubsidizeCommand request, CancellationToken cancellationToken)
    {
        var CurrentSubsidize = await _context.Get<Domain.Entities.Subsidize>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentSubsidize == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Subsidize), request.Id);
        }
        CurrentSubsidize.IsDeleted = true;



        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

