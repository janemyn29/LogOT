using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Attendance.Commands.DeleteAttendance;

public record DeleteAttendanceCommand : IRequest<bool>
{
    public Guid Id { get; init; }

}
public class DeleteAttendanceCommandHandler : IRequestHandler<DeleteAttendanceCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteAttendanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
    {
        var CurrentAttendance = await _context.Get<Domain.Entities.Attendance>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentAttendance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Attendance), request.Id);
        }
        CurrentAttendance.IsDeleted = true;



        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
