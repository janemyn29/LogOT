using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Positions.Queries.GetPosition;
public class GetListPositionRequest : IRequest<PaginatedList<Domain.Entities.Position>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListPositionRequestHandler : IRequestHandler<GetListPositionRequest, PaginatedList<Domain.Entities.Position>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListPositionRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<Domain.Entities.Position>> Handle(GetListPositionRequest request, CancellationToken cancellationToken)
    {
        var ListCity = _context.Get<Domain.Entities.Position>().Include(x=>x.Department).Include(x=>x.Level).Where(x => x.IsDeleted == false).AsNoTracking();
        var page = await PaginatedList<Domain.Entities.Position>
            .CreateAsync(ListCity, request.Page, request.Size);

        return page;
    }
}

