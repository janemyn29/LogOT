using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;

namespace mentor_v1.Application.ShiftConfig.Queries;
public class GetShiftDayByShiftRequest : IRequest<Domain.Entities.ShiftConfig>
{
    public DateTime AttendanceTime { get; set; }
}

// IRequestHandler<request type, return type>
public class GetShiftDayByShiftRequestHandler : IRequestHandler<GetShiftDayByShiftRequest, Domain.Entities.ShiftConfig>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetShiftDayByShiftRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.ShiftConfig> Handle(GetShiftDayByShiftRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var shift = _context.Get<Domain.Entities.ShiftConfig>().Where(x => x.IsDeleted == false && x.StartTime.Value.AddMinutes(-30).TimeOfDay <= request.AttendanceTime.TimeOfDay && x.EndTime.Value.TimeOfDay > request.AttendanceTime.TimeOfDay ).FirstOrDefault();
        if (shift == null)
        {
            throw new NotFoundException("Hiện đang không trong ca làm việc nên bạn không thể điểm danh được!");
        }
        return Task.FromResult(shift); //Task.CompletedTask;
    }
}