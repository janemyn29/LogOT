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

namespace mentor_v1.Application.Experience.Queries.GetExperienceWithRelativeObject;

public class GetListExperienceByApplicationUserIdRequest : IRequest<PaginatedList<ExperienceViewModel>>
{
    public string Id { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }

}

public class GetListExperienceByApplicationUserIdRequestHandler : IRequestHandler<GetListExperienceByApplicationUserIdRequest, PaginatedList<ExperienceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListExperienceByApplicationUserIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<ExperienceViewModel>> Handle(GetListExperienceByApplicationUserIdRequest request, CancellationToken cancellationToken)
    {

        //get Experience by ?
        var Experiences = _applicationDbContext.Get<Domain.Entities.Experience>().Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.Id)).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<ExperienceViewModel>(Experiences);

        var page = PaginatedList<ExperienceViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}