using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Experience.Queries.GetExperienceWithRelativeObject;
public class GetExperienceByIdRequest : IRequest<GetExperience.ExperienceViewModel>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetExperienceByIdRequestHandler : IRequestHandler<GetExperienceByIdRequest, GetExperience.ExperienceViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetExperienceByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetExperience.ExperienceViewModel> Handle(GetExperienceByIdRequest request, CancellationToken cancellationToken)
    {
        var Experience = _context.Get<Domain.Entities.Experience>()          
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (Experience == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Experience), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        var map = _mapper.Map<GetExperience.ExperienceViewModel>(Experience);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}