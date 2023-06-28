using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
public class GetListAttendanceByUserRequest : IRequest<PaginatedList<AttendanceViewModel>>
{
    public string UserId { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListAttendanceByUserRequestHandler : IRequestHandler<GetListAttendanceByUserRequest, PaginatedList<AttendanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAttendanceByUserRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<AttendanceViewModel>> Handle(GetListAttendanceByUserRequest request, CancellationToken cancellationToken)
    {
        //get Attendance by ?
        var Attendances = _applicationDbContext.Get<Domain.Entities.Attendance>().Where(x => x.IsDeleted == false && x.ApplicationUserId == request.UserId).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<AttendanceViewModel>(Attendances);
        var page = PaginatedList<AttendanceViewModel>.CreateAsync(models, request.Page, request.Size);
        return page;
    }
}
