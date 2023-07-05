using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.OvertimeLog.Commands.DeleteOvertimeLog;

public record DeleteOvertimeLogCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteOvertimeLogCommandHandler : IRequestHandler<DeleteOvertimeLogCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteOvertimeLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteOvertimeLogCommand request, CancellationToken cancellationToken)
    {
        /*var user = await _context.Get<Domain.Identity.ApplicationUser>().Where(p => p.over == request.Id && p.IsDeleted == false).FirstOrDefaultAsync();

        if (positions != null)
        {
            throw new Exception();
        }*/

        var CurrentOvertimeLog = await _context.Get<Domain.Entities.OvertimeLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentOvertimeLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OvertimeLog), request.Id);
        }

        CurrentOvertimeLog.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}