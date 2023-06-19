using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Experience.Queries.GetExperience;

namespace mentor_v1.Application.Experience.Queries.GetExperience;

public class GetListExperienceRequest : IRequest<PaginatedList<ExperienceViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListExperienceRequestHandler : IRequestHandler<GetListExperienceRequest, PaginatedList<ExperienceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListExperienceRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<ExperienceViewModel>> Handle(GetListExperienceRequest request, CancellationToken cancellationToken)
    {

        //get Experience by ?
        var Experiences = _applicationDbContext.Get<Domain.Entities.Experience>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<ExperienceViewModel>(Experiences);

        var page = PaginatedList<ExperienceViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}