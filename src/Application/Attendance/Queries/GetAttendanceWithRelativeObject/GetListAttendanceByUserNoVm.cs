using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
public class GetListAttendanceByUserNoVm : IRequest<List<Domain.Entities.Attendance>>
{
    public string UserId { get; set; }
}

public class GetListAttendanceByUserNoVmHandler : IRequestHandler<GetListAttendanceByUserNoVm, List<Domain.Entities.Attendance>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAttendanceByUserNoVmHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.Attendance>> Handle(GetListAttendanceByUserNoVm request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var Attendances = _applicationDbContext.Get<Domain.Entities.Attendance>().Where(x => x.IsDeleted == false && x.ApplicationUserId == request.UserId).OrderByDescending(x => x.Day).AsNoTracking().ToList();
        return Task.FromResult(Attendances);
    }
}
