using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;

public class GetLeaveLogByIdRequest : IRequest<LeaveLogViewModel>
{
    public Guid Id { get; set; }
}

// IRequestHandler<request type, return type>
public class GetLeaveLogByIdRequestHandler : IRequestHandler<GetLeaveLogByIdRequest, LeaveLogViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetLeaveLogByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<LeaveLogViewModel> Handle(GetLeaveLogByIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
            var leaveLog = _context.Get<Domain.Entities.LeaveLog>()
            .Include(x => x.ApplicationUser)
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
        var map = _mapper.Map<LeaveLogViewModel>(leaveLog);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}