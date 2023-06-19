using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Experience.Commands.CreateExperience;


public class CreateExperienceCommand : IRequest<Guid>
{
    public string ApplicationUserId { get; set; }
    public string NameProject { get; set; }
    public int TeamSize { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string TechStack { get; set; }


}

public class CreateExperienceCommandHandler : IRequestHandler<CreateExperienceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateExperienceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateExperienceCommand request, CancellationToken cancellationToken)
    {

        var Experience = new Domain.Entities.Experience()
        {
            ApplicationUserId = request.ApplicationUserId,
            NameProject = request.NameProject,
            TeamSize = request.TeamSize,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            TechStack = request.TechStack,

        };

        _context.Get<Domain.Entities.Experience>().Add(Experience);

        await _context.SaveChangesAsync(cancellationToken);

        return Experience.Id;
    }
}

