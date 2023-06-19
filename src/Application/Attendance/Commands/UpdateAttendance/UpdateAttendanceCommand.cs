using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Attendance.Commands.UpdateAttendance;

public record UpdateAttendanceCommand : IRequest
{
    public Guid Id { get; init; }
    public string ApplicationUserId { get; set; }
    public DateTime Day { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ShiftEnum ShiftEnum { get; set; }

}
public class UpdateAttendanceCommandHandler : IRequestHandler<UpdateAttendanceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAttendanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var CurrentAttendance = await _context.Get<Domain.Entities.Attendance>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentAttendance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Attendance), request.Id);
        }

        CurrentAttendance.ApplicationUserId = request.ApplicationUserId;
        CurrentAttendance.Day = request.Day;
        CurrentAttendance.StartTime = request.StartTime;
        CurrentAttendance.EndTime = request.EndTime;
        CurrentAttendance.ShiftEnum = request.ShiftEnum;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
