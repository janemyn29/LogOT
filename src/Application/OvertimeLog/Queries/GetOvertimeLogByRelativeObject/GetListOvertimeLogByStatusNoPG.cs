using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using mentor_v1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;

public class GetListOvertimeLogByStatusNoPG : IRequest<List<OvertimeLogViewModel>>
{
    public LogStatus status { get; set; }
    //public int Page { get; set; }
    //public int Size { get; set; }
}

public class GetListOvertimeLogByStatusNoPGHandler : IRequestHandler<GetListOvertimeLogByStatusNoPG, List<OvertimeLogViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListOvertimeLogByStatusNoPGHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<List<OvertimeLogViewModel>> Handle(GetListOvertimeLogByStatusNoPG request, CancellationToken cancellationToken)
    {

        //get OvertimeLog 
        var OvertimeLogs = _applicationDbContext.Get<Domain.Entities.OvertimeLog>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false && x.Status.Equals(request.status))
            .OrderByDescending(x => x.Date).AsNoTracking();

         var models = _mapper.ProjectTo<OvertimeLogViewModel>(OvertimeLogs);

        return Task.FromResult(models.ToList());
    }
}