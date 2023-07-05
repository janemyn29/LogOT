using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Level.Commands.DeleteLevel;

public record DeleteLevelCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteLevelCommandHandler : IRequestHandler<DeleteLevelCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteLevelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteLevelCommand request, CancellationToken cancellationToken)
    {
        var positions = _context.Get<Domain.Entities.Position>().Where(p => p.LevelId.Equals(request.Id) && p.IsDeleted == false).AsNoTracking().FirstOrDefault();

        if (positions != null) 
        {
            throw new Exception();
        }

        var CurrentLevel = await _context.Get<Domain.Entities.Level>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentLevel == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Level), request.Id);
        }

        CurrentLevel.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}