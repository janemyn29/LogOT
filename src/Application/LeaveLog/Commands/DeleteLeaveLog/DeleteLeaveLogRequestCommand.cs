using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.LeaveLog.Commands.DeleteLeaveLog;

public record DeleteLeaveLogCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteLeaveLogCommandHandler : IRequestHandler<DeleteLeaveLogCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteLeaveLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteLeaveLogCommand request, CancellationToken cancellationToken)
    {
        /*var user = await _context.Get<Domain.Identity.ApplicationUser>().Where(p => p.over == request.Id && p.IsDeleted == false).FirstOrDefaultAsync();

        if (positions != null)
        {
            throw new Exception();
        }*/

        var CurrentLeaveLog = await _context.Get<Domain.Entities.LeaveLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentLeaveLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.LeaveLog), request.Id);
        }

        CurrentLeaveLog.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
