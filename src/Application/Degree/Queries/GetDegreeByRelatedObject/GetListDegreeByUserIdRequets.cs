using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Degree.Queries.GetDegree;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Degree.Queries.GetDegreeByRelatedObject;
public class GetListDegreeByUserIdRequets : IRequest<PaginatedList<GetDegreeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string UserId { get; set; }


}

public class GetListDegreeByUserIdRequetsHandler : IRequestHandler<GetListDegreeByUserIdRequets, PaginatedList<GetDegreeViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetListDegreeByUserIdRequetsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<GetDegreeViewModel>> Handle(GetListDegreeByUserIdRequets request, CancellationToken cancellationToken)
    {
        var degree = _context.Get<Domain.Entities.Degree>().Where(x => x.IsDeleted == false && x.ApplicationUserId.ToLower() ==request.UserId.ToLower() ).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetDegreeViewModel>(degree);
        var page = PaginatedList<GetDegreeViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}
