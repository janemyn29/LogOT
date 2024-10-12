using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using System.Data.Entity;

namespace mentor_v1.Application.Attendance.Queries.GetAttendance;

public class GetListAttendanceRequest : IRequest<PaginatedList<AttendanceViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListAttendanceRequestHandler : IRequestHandler<GetListAttendanceRequest, PaginatedList<AttendanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAttendanceRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public Task<PaginatedList<AttendanceViewModel>> Handle(GetListAttendanceRequest request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var Attendances = _applicationDbContext.Get<Domain.Entities.Attendance>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<AttendanceViewModel>(Attendances);

        var page = PaginatedList<AttendanceViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}