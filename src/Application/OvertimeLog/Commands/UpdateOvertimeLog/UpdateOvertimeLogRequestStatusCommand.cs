using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;

public record UpdateOvertimeLogRequestStatusCommand : IRequest
{
    public Guid Id { get; init; }
    public LogStatus status { get; init; }
    public string? cancelReason { get; init; }
}
public class UpdateOvertimeLogRequestStatusCommandHandler : IRequestHandler<UpdateOvertimeLogRequestStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateOvertimeLogRequestStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOvertimeLogRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var CurrentOvertimeLog = await _context.Get<Domain.Entities.OvertimeLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentOvertimeLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OvertimeLog), request.Id);
        }

        CurrentOvertimeLog.Status = request.status;
        CurrentOvertimeLog.CancelReason = request.cancelReason;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}