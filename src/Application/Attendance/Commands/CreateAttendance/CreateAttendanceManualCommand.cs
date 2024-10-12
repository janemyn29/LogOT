using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Attendance.Commands.CreateAttendance;
public class CreateAttendanceManualCommand : IRequest<Guid>
{
    public string ApplicationUserId { get; set; }
    public DateTime Day { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ShiftEnum ShiftEnum { get; set; }

    public double WorkHour { get; set; }
    public double OtHour { get; set; }


}

public class CreateAttendanceManualCommandHandler : IRequestHandler<CreateAttendanceManualCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAttendanceManualCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAttendanceManualCommand request, CancellationToken cancellationToken)
    {

        var Attendance = new Domain.Entities.Attendance()
        {
            ApplicationUserId = request.ApplicationUserId,
            Day = request.Day,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            ShiftEnum = request.ShiftEnum,
            TimeWork = request.WorkHour,
            OverTime = request.OtHour

        };

        _context.Get<Domain.Entities.Attendance>().Add(Attendance);

        await _context.SaveChangesAsync(cancellationToken);

        return Attendance.Id;
    }
}

