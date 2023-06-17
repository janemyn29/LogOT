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
using mentor_v1.Application.Subsidize.Queries.GetSubsidize;

namespace mentor_v1.Application.Subsidize.Queries.GetSubsidize;

public class GetListSubsidizeRequest : IRequest<PaginatedList<SubsidizeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListSubsidizeRequestHandler : IRequestHandler<GetListSubsidizeRequest, PaginatedList<SubsidizeViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListSubsidizeRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<SubsidizeViewModel>> Handle(GetListSubsidizeRequest request, CancellationToken cancellationToken)
    {

        //get Subsidize by ?
        var Subsidizes = _applicationDbContext.Get<Domain.Entities.Subsidize>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<SubsidizeViewModel>(Subsidizes);

        var page = PaginatedList<SubsidizeViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}