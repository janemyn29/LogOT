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

namespace mentor_v1.Application.Allowance.Queries.GetAllowance;
public class GetAllowanceRequest : IRequest<PaginatedList<AllowanceViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetAllowanceRequestHandler : IRequestHandler<GetAllowanceRequest, PaginatedList<AllowanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetAllowanceRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public Task<PaginatedList<AllowanceViewModel>> Handle(GetAllowanceRequest request, CancellationToken cancellationToken)
    {
        var allowance = _applicationDbContext.Get<Domain.Entities.Allowance>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<AllowanceViewModel>(allowance);
        var page = PaginatedList<AllowanceViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}