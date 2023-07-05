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

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;

public class GetLeaveLogByIdRequest : IRequest<Domain.Entities.LeaveLog>
{
    public Guid Id { get; set; }
    

}

// IRequestHandler<request type, return type>
public class GetLeaveLogByIdRequestHandler : IRequestHandler<GetLeaveLogByIdRequest, Domain.Entities.LeaveLog>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetLeaveLogByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.LeaveLog> Handle(GetLeaveLogByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
            var leaveLog = _context.Get<Domain.Entities.LeaveLog>()
           .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
           .AsNoTracking().FirstOrDefault();
        
        /*var LeaveLog = _context.Get<Domain.Entities.LeaveLog>()
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();*/
        if (leaveLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.LeaveLog), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        //var map = _mapper.Map<GetLeaveLog.LeaveLogViewModel>(LeaveLog);

        // Paginate data
        return Task.FromResult(leaveLog); //Task.CompletedTask;
    }
}