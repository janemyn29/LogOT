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
public class GetAttendaceByUserAndShift : IRequest<Domain.Entities.Attendance>
{
    public string  userId { get; set; }
    public ShiftEnum ShiftEnum { get; set; }
    public DateTime Day { get; set; }
}

// IRequestHandler<request type, return type>
public class GetAttendaceByUserAndShiftHandler : IRequestHandler<GetAttendaceByUserAndShift, Domain.Entities.Attendance>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetAttendaceByUserAndShiftHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.Attendance> Handle(GetAttendaceByUserAndShift request, CancellationToken cancellationToken)
    {
        var Attendance = _context.Get<Domain.Entities.Attendance>()
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.userId) && x.ShiftEnum == request.ShiftEnum && x.Day.Date == request.Day.Date)
            .AsNoTracking().FirstOrDefault();
        

       

        // Paginate data
        return Task.FromResult(Attendance); //Task.CompletedTask;
    }
}
