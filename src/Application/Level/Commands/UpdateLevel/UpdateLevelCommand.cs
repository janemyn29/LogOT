using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Level.Commands.UpdateLevel;

public record UpdateLevelCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; set; }

    public string Description { get; set; }

    public IList<Position> Positions { get; set; }
}
public class UpdateLevelCommandHandler : IRequestHandler<UpdateLevelCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLevelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLevelCommand request, CancellationToken cancellationToken)
    {
        var CurrentLevel = await _context.Get<Domain.Entities.Level>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentLevel == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Level), request.Id);
        }
        
        CurrentLevel.Name = request.Name;
        CurrentLevel.Description = request.Description;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}