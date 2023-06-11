using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Allowance.Queries.GetAllowance;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;

namespace mentor_v1.Application.Degree.Queries.GetDegree;
public class GetDegreeRequest : IRequest<PaginatedList<GetDegreeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }

}

public class GetDegreeRequestHandler : IRequestHandler<GetDegreeRequest, PaginatedList<GetDegreeViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDegreeRequestHandler (IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<GetDegreeViewModel>> Handle(GetDegreeRequest request, CancellationToken cancellationToken)
    {
        var degree = _context.Get<Domain.Entities.Degree>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetDegreeViewModel>(degree);
        var page = PaginatedList<GetDegreeViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}
