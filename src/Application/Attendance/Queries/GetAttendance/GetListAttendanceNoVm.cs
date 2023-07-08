using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Attendance.Queries.GetAttendance;
public class GetListAttendanceNoVm : IRequest<List<AttendanceViewModel>>
{
}

public class GetListAttendanceNoVmHandler : IRequestHandler<GetListAttendanceNoVm, List<AttendanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAttendanceNoVmHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public Task<List<AttendanceViewModel>> Handle(GetListAttendanceNoVm request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var Attendances = _applicationDbContext.Get<Domain.Entities.Attendance>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<AttendanceViewModel>(Attendances).ToList();

        return Task.FromResult(models);
    }
}
