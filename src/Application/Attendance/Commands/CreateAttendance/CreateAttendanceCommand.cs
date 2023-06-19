using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Attendance.Commands.CreateAttendance;

public class CreateAttendanceCommand : IRequest<Guid>
{
    public string ApplicationUserId { get; set; }
    public DateTime Day { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ShiftEnum ShiftEnum { get; set; }


}

public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAttendanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {

        var Attendance = new Domain.Entities.Attendance()
        {
            ApplicationUserId = request.ApplicationUserId,
            Day = request.Day,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            ShiftEnum = request.ShiftEnum,

        };

        _context.Get<Domain.Entities.Attendance>().Add(Attendance);

        await _context.SaveChangesAsync(cancellationToken);

        return Attendance.Id;
    }
}
