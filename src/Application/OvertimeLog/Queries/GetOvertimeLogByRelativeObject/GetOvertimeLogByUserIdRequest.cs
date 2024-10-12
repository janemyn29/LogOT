using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLogByRelativeObject;

public class GetOvertimeLogByUserIdRequest : IRequest<PaginatedList<OvertimeLogViewModel>>
{
    public string id { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetOvertimeLogByUserIdRequestHandler : IRequestHandler<GetOvertimeLogByUserIdRequest, PaginatedList<OvertimeLogViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetOvertimeLogByUserIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<OvertimeLogViewModel>> Handle(GetOvertimeLogByUserIdRequest request, CancellationToken cancellationToken)
    {

        //get OvertimeLog 
        var OvertimeLogs = _applicationDbContext.Get<Domain.Entities.OvertimeLog>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.id)).OrderByDescending(x => x.Date).AsNoTracking();
        var models = _mapper.ProjectTo<OvertimeLogViewModel>(OvertimeLogs);

        var page = PaginatedList<OvertimeLogViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}