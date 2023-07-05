using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;

public record UpdateLeaveLogRequestStatusCommand : IRequest
{
    public Guid Id { get; init; }
    public LogStatus status { get; init; }
    public string? cancelReason { get; init; }
}
public class UpdateLeaveLogRequestStatusCommandHandler : IRequestHandler<UpdateLeaveLogRequestStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLeaveLogRequestStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLeaveLogRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var CurrentLeaveLog = await _context.Get<Domain.Entities.LeaveLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentLeaveLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.LeaveLog), request.Id);
        }

        CurrentLeaveLog.Status = request.status;
        CurrentLeaveLog.CancelReason = request.cancelReason;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}