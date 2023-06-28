using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;

namespace mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;

public class CreateLeaveLogCommand : IRequest<Guid>
{
    public CreateLeaveLogViewModel createLeaveLogViewModel { get; set; }

}

// Handler to handle the request (Can be written to another file)
// CreateLeaveLogCommand : IRequest<Guid> => IRequestHandler<CreateLeaveLogCommand, Guid>
public class CreateLeaveLogCommandHandler : IRequestHandler<CreateLeaveLogCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLeaveLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLeaveLogCommand request, CancellationToken cancellationToken)
    {
        /*if (request.createLeaveLogViewModel.StartDate < DateTime.UtcNow)
        {
            throw new Exception("Ngày yêu cầu không thể trước thời gian hiện tại");
        } else if (request.createLeaveLogViewModel.StartDate > request.createLeaveLogViewModel.EndDate)
        {
            throw new Exception("Ngày bắt đầu phải trước ngày kết thúc");
        }*/

        // create new LeaveLog from request data
        var LeaveLog = new Domain.Entities.LeaveLog()
        {
            ApplicationUserId = request.createLeaveLogViewModel.ApplicationUserId,
            StartDate = request.createLeaveLogViewModel.StartDate,
            EndDate = request.createLeaveLogViewModel.EndDate,
            LeaveHours = request.createLeaveLogViewModel.LeaveHours,
            Reason = request.createLeaveLogViewModel.Reason,
            Status = Domain.Enums.LogStatus.Request
        };

        // add new LeaveLog
        _context.Get<Domain.Entities.LeaveLog>().Add(LeaveLog);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        // return the Guid
        return LeaveLog.Id;
    }
}