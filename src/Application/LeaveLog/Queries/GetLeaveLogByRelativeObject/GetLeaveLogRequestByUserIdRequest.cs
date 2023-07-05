using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;

public class GetLeaveLogRequestByUserIdRequest : IRequest<Domain.Entities.LeaveLog>
{
    public string UserId { get; set; }
    public DateTime day { get; set; }

}

// IRequestHandler<request type, return type>
public class GetLeaveLogRequestByUserIdRequestHandler : IRequestHandler<GetLeaveLogRequestByUserIdRequest, Domain.Entities.LeaveLog>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetLeaveLogRequestByUserIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.LeaveLog> Handle(GetLeaveLogRequestByUserIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var LeaveLog = _context.Get<Domain.Entities.LeaveLog>()
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.UserId) && x.LeaveDate.Date == request.day.Date && x.Status == Domain.Enums.LogStatus.Approved)
            .AsNoTracking().FirstOrDefault();
        return Task.FromResult(LeaveLog); //Task.CompletedTask;
    }
}