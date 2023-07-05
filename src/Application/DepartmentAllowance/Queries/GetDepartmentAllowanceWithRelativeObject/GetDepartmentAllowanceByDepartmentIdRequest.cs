using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowanceWithRelativeObject;

public class GetDepartmentAllowanceByDepartmentIdRequest : IRequest<List<Domain.Entities.DepartmentAllowance>>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetDepartmentAllowanceByDepartmentIdRequestHandler : IRequestHandler<GetDepartmentAllowanceByDepartmentIdRequest, List<Domain.Entities.DepartmentAllowance>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetDepartmentAllowanceByDepartmentIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.DepartmentAllowance>> Handle(GetDepartmentAllowanceByDepartmentIdRequest request, CancellationToken cancellationToken)
    {
        var DepartmentAllowance = _context.Get<Domain.Entities.DepartmentAllowance>().Include(x=>x.Subsidize)
            .Where(x => x.IsDeleted == false && x.DepartmentId.Equals(request.Id))
            .AsNoTracking().ToList();
        // Paginate data
        return Task.FromResult(DepartmentAllowance); //Task.CompletedTask;
    }
}
