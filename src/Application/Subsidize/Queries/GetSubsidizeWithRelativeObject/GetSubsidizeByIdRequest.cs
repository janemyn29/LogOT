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

namespace mentor_v1.Application.Subsidize.Queries.GetSubsidizeWithRelativeObject;


public class GetSubsidizeByIdRequest : IRequest<GetSubsidize.SubsidizeViewModel>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetSubsidizeByIdRequestHandler : IRequestHandler<GetSubsidizeByIdRequest, GetSubsidize.SubsidizeViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetSubsidizeByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetSubsidize.SubsidizeViewModel> Handle(GetSubsidizeByIdRequest request, CancellationToken cancellationToken)
    {
        var Subsidize = _context.Get<Domain.Entities.Subsidize>()
            .Include(x => x.DepartmentAllowances)
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (Subsidize == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Subsidize), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        var map = _mapper.Map<GetSubsidize.SubsidizeViewModel>(Subsidize);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}