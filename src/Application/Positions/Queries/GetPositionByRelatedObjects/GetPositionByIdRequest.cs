using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Positions.Queries.GetPositionByRelatedObjects;

public class GetPositionByIdRequest : IRequest<Domain.Entities.Position>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetPositionByIdRequestHandler : IRequestHandler<GetPositionByIdRequest, Domain.Entities.Position>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetPositionByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.Position> Handle(GetPositionByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var city = _context.Get<Domain.Entities.Position>().Include(x=>x.ApplicationUsers).Include(x=>x.Department).Include(x=>x.Level).Where(x => x.IsDeleted == false && x.Id== request.Id).AsNoTracking().FirstOrDefault();
        if (city == null)
        {
            throw new NotFoundException("Không tìm vị trí công việc mà bạn yêu cầu!");
        }

        // Paginate data
        return Task.FromResult(city); //Task.CompletedTask;
    }
}

