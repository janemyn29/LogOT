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

namespace mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;
public class GetListAnnualByDayToDayRequest : IRequest<List<Domain.Entities.AnnualWorkingDay>>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class GetListAnnualByDayToDayRequestHandler : IRequestHandler<GetListAnnualByDayToDayRequest, List<Domain.Entities.AnnualWorkingDay>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListAnnualByDayToDayRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public Task<List<Domain.Entities.AnnualWorkingDay>> Handle(GetListAnnualByDayToDayRequest request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var item = _applicationDbContext.Get<Domain.Entities.AnnualWorkingDay>().Include(x=>x.Coefficient).Where(x => x.IsDeleted == false && x.Day.Date>= request.FromDate && x.Day.Date<=request.ToDate).OrderBy(x => x.Day).AsNoTracking().ToList();
        return Task.FromResult(item);
    }
}
