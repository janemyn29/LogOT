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
public class GetListAttendanceByUserNoPaging : IRequest<List<AttendanceViewModel>>
{
    public string UserId { get; set; }
}

public class GetListAttendanceByUserNoPagingHandler : IRequestHandler<GetListAttendanceByUserNoPaging, List<AttendanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAttendanceByUserNoPagingHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<AttendanceViewModel>> Handle(GetListAttendanceByUserNoPaging request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var Attendances = _applicationDbContext.Get<Domain.Entities.Attendance>().Where(x => x.IsDeleted == false && x.ApplicationUserId == request.UserId).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<AttendanceViewModel>(Attendances).ToList();
        return models;
    }
}
