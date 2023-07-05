using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;

public record UpdateLeaveLogCommand : IRequest
{
    public Guid Id { get; init; }
    public UpdateLeaveLogViewModel updateLeaveLogViewModel { get; init; }
}
public class UpdateLeaveLogCommandHandler : IRequestHandler<UpdateLeaveLogCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLeaveLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLeaveLogCommand request, CancellationToken cancellationToken)
    {
        /*if (request.updateLeaveLogViewModel.StartDate < DateTime.UtcNow)
        {
            throw new Exception("Ngày yêu cầu không thể trước thời gian hiện tại");
        }
        else if (request.updateLeaveLogViewModel.StartDate > request.updateLeaveLogViewModel.EndDate)
        {
            throw new Exception("Ngày bắt đầu phải trước ngày kết thúc");
        }*/

        var CurrentLeaveLog = await _context.Get<Domain.Entities.LeaveLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentLeaveLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.LeaveLog), request.Id);
        }

       /* CurrentLeaveLog.StartDate = request.updateLeaveLogViewModel.StartDate;
        CurrentLeaveLog.EndDate = request.updateLeaveLogViewModel.EndDate;
        CurrentLeaveLog.LeaveHours = request.updateLeaveLogViewModel.LeaveHours;
        CurrentLeaveLog.Reason = request.updateLeaveLogViewModel.Reason;*/
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}