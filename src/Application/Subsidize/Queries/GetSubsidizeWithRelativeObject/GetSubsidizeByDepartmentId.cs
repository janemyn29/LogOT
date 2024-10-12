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

namespace mentor_v1.Application.Subsidize.Queries.GetSubsidizeWithRelativeObject;
public class GetSubsidizeByDepartmentId : IRequest<Domain.Entities.Subsidize>
{ }
/*{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetSubsidizeByDepartmentIdHandler : IRequestHandler<GetSubsidizeByDepartmentId, Domain.Entities.Subsidize>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetSubsidizeByDepartmentIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.Subsidize> Handle(GetSubsidizeByDepartmentId request, CancellationToken cancellationToken)
    {
        var Subsidize = _context.Get<Domain.Entities.Subsidize>()
            .Include(x => x.DepartmentAllowances)
            .Where(x => x.IsDeleted == false && x.DepartmentAllowances.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (Subsidize == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Subsidize), request.Id);
        }


        // Paginate data
        return Task.FromResult(Subsidize); //Task.CompletedTask;
    }
}*/
