using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
public class GetAttendaceByUserAndShift : IRequest<GetAttendance.AttendanceViewModel>
{
    public string  userId { get; set; }
    public ShiftEnum ShiftEnum { get; set; }
    public DateTime Day { get; set; }
}

// IRequestHandler<request type, return type>
public class GetAttendaceByUserAndShiftHandler : IRequestHandler<GetAttendaceByUserAndShift, GetAttendance.AttendanceViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetAttendaceByUserAndShiftHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetAttendance.AttendanceViewModel> Handle(GetAttendaceByUserAndShift request, CancellationToken cancellationToken)
    {
        var Attendance = _context.Get<Domain.Entities.Attendance>()
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.userId) && x.ShiftEnum == request.ShiftEnum && x.Day.Date == request.Day.Date)
            .AsNoTracking().FirstOrDefault();
        if (Attendance == null)
        {
            throw new NotFoundException("Không tìm thấy");
        }

        // AsNoTracking to remove default tracking on entity framework
        var map = _mapper.Map<GetAttendance.AttendanceViewModel>(Attendance);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}
