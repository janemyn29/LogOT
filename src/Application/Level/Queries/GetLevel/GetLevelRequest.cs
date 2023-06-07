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
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Level.Queries.GetLevel;

public class GetLevelRequest : IRequest<PaginatedList<LevelViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetLevelRequestHandler : IRequestHandler<GetLevelRequest, PaginatedList<LevelViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetLevelRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<LevelViewModel>> Handle(GetLevelRequest request, CancellationToken cancellationToken)
    {

        //get Level by ?
        var Levels = _applicationDbContext.Get<Domain.Entities.Level>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<LevelViewModel>(Levels);

        var page = PaginatedList<LevelViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}