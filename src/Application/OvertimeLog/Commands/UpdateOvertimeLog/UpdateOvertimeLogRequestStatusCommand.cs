using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

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
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public UpdateOvertimeLogRequestStatusCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager, IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _mediator = mediator;
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

        var listManager = await _userManager.GetUsersInRoleAsync("Manager");

        

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}