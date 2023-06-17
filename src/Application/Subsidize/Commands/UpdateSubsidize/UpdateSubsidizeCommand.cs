using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Subsidize.Commands.UpdateSubsidize;

public record UpdateSubsidizeCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; set; }

    public string Description { get; set; }

    public double Amount { get; set; }

}
public class UpdateSubsidizeCommandHandler : IRequestHandler<UpdateSubsidizeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSubsidizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSubsidizeCommand request, CancellationToken cancellationToken)
    {
        var CurrentSubsidize = await _context.Get<Domain.Entities.Subsidize>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentSubsidize == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Subsidize), request.Id);
        }

        CurrentSubsidize.Name = request.Name;
        CurrentSubsidize.Description = request.Description;
        CurrentSubsidize.Amount = request.Amount;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}