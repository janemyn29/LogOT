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

namespace mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;

public class GetListAttendanceByApplicationUserIdRequest : IRequest<PaginatedList<AttendanceViewModel>>
{
    public string Id { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }

}

public class GetListAttendanceByApplicationUserIdRequestHandler : IRequestHandler<GetListAttendanceByApplicationUserIdRequest, PaginatedList<AttendanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAttendanceByApplicationUserIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<AttendanceViewModel>> Handle(GetListAttendanceByApplicationUserIdRequest request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var Attendances = _applicationDbContext.Get<Domain.Entities.Attendance>().Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.Id)).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<AttendanceViewModel>(Attendances);

        var page = PaginatedList<AttendanceViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}
