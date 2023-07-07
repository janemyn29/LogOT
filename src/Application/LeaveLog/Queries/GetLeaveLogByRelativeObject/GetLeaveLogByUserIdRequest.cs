using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;
public class GetLeaveLogByUserIdRequest : IRequest<LeaveLogViewModel>
{
    public string UserId { get; set; }
    public DateTime day { get; set; }

}

// IRequestHandler<request type, return type>
public class GetLeaveLogByUserIdRequestHandler : IRequestHandler<GetLeaveLogByUserIdRequest, LeaveLogViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetLeaveLogByUserIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<LeaveLogViewModel> Handle(GetLeaveLogByUserIdRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var LeaveLog = _context.Get<Domain.Entities.LeaveLog>()
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.UserId) && x.LeaveDate.Date == request.day.Date  && x.Status == Domain.Enums.LogStatus.Approved)
            .AsNoTracking().FirstOrDefault();
        var map = _mapper.Map<LeaveLogViewModel>(LeaveLog);
        return Task.FromResult(map); //Task.CompletedTask;
    }
}