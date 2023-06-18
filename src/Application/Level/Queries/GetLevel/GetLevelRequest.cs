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

public class GetLevelRequest : IRequest<PaginatedList<Domain.Entities.Level>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetLevelRequestHandler : IRequestHandler<GetLevelRequest, PaginatedList<Domain.Entities.Level>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetLevelRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.Level>> Handle(GetLevelRequest request, CancellationToken cancellationToken)
    {

        //get Level by ?
        var Levels = _applicationDbContext.Get<Domain.Entities.Level>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        //var models = _mapper.ProjectTo<LevelViewModel>(Levels);

        var page = PaginatedList<Domain.Entities.Level>.CreateAsync(Levels, request.Page, request.Size);

        return page;
    }
}