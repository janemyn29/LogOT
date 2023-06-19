using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Experience.Commands.UpdateExperience;

public record UpdateExperienceCommand : IRequest
{
    public Guid Id { get; init; }
    public string ApplicationUserId { get; set; }
    public string NameProject { get; set; }
    public int TeamSize { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string TechStack { get; set; }

}
public class UpdateExperienceCommandHandler : IRequestHandler<UpdateExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
    {
        var CurrentExperience = await _context.Get<Domain.Entities.Experience>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentExperience == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Experience), request.Id);
        }

        CurrentExperience.ApplicationUserId = request.ApplicationUserId;
        CurrentExperience.NameProject = request.NameProject;
        CurrentExperience.TeamSize = request.TeamSize;
        CurrentExperience.StartDate = request.StartDate;
        CurrentExperience.EndDate = request.EndDate;
        CurrentExperience.Description = request.Description;
        CurrentExperience.TechStack = request.TechStack;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}