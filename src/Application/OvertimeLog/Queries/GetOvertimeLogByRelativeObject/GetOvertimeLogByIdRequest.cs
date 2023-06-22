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

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;

public class GetOvertimeLogByIdRequest : IRequest<Domain.Entities.OvertimeLog>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetOvertimeLogByIdRequestHandler : IRequestHandler<GetOvertimeLogByIdRequest, Domain.Entities.OvertimeLog>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetOvertimeLogByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.OvertimeLog> Handle(GetOvertimeLogByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var OvertimeLog = _context.Get<Domain.Entities.OvertimeLog>()
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (OvertimeLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OvertimeLog), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        //var map = _mapper.Map<GetOvertimeLog.OvertimeLogViewModel>(OvertimeLog);

        // Paginate data
        return Task.FromResult(OvertimeLog); //Task.CompletedTask;
    }
}