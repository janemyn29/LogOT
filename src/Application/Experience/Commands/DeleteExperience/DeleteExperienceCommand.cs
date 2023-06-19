using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Experience.Commands.DeleteExperience;
public record DeleteExperienceCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteExperienceCommandHandler : IRequestHandler<DeleteExperienceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
    {
        var CurrentExperience = await _context.Get<Domain.Entities.Experience>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentExperience == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Experience), request.Id);
        }
        CurrentExperience.IsDeleted = true;



        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
