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
using mentor_v1.Application.Degree.Queries.GetDegree;

namespace mentor_v1.Application.Dependent.Queries;
public class GetDependentRequest : IRequest<PaginatedList<GetDependentViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }

}

public class GetDependentRequestHandler : IRequestHandler<GetDependentRequest, PaginatedList<GetDependentViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDependentRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<GetDependentViewModel>> Handle(GetDependentRequest request, CancellationToken cancellationToken)
    {
        var dependent = _context.Get<Domain.Entities.Dependent>().Where(x => x.IsDeleted == false).Include(x => x.ApplicationUser).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetDependentViewModel>(dependent);
        var page = PaginatedList<GetDependentViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}
