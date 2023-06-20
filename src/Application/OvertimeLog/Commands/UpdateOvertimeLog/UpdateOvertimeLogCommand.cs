using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;

namespace mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;

public record UpdateOvertimeLogCommand : IRequest
{
    public Guid Id { get; init; }
    public UpdateOvertimeLogViewModel updateOvertimeLogViewModel { get; init; }
}
public class UpdateOvertimeLogCommandHandler : IRequestHandler<UpdateOvertimeLogCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateOvertimeLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOvertimeLogCommand request, CancellationToken cancellationToken)
    {
        if (request.updateOvertimeLogViewModel.Date < DateTime.UtcNow)
        {
            throw new Exception("Thời gian yêu cầu không thể trước thời gian hiện tại");
        }

        var CurrentOvertimeLog = await _context.Get<Domain.Entities.OvertimeLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentOvertimeLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OvertimeLog), request.Id);
        }

        CurrentOvertimeLog.Date = request.updateOvertimeLogViewModel.Date;
        CurrentOvertimeLog.Hours = request.updateOvertimeLogViewModel.Hours;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}