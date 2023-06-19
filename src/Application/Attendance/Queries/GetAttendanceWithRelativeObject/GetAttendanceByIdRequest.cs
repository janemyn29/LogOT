﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
public class GetAttendanceByIdRequest : IRequest<GetAttendance.AttendanceViewModel>
{
    public Guid Id { get; set; }

}

// IRequestHandler<request type, return type>
public class GetAttendanceByIdRequestHandler : IRequestHandler<GetAttendanceByIdRequest, GetAttendance.AttendanceViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetAttendanceByIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetAttendance.AttendanceViewModel> Handle(GetAttendanceByIdRequest request, CancellationToken cancellationToken)
    {
        var Attendance = _context.Get<Domain.Entities.Attendance>()
            .Where(x => x.IsDeleted == false && x.Id.Equals(request.Id))
            .AsNoTracking().FirstOrDefault();
        if (Attendance == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Attendance), request.Id);
        }

        // AsNoTracking to remove default tracking on entity framework
        var map = _mapper.Map<GetAttendance.AttendanceViewModel>(Attendance);

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}