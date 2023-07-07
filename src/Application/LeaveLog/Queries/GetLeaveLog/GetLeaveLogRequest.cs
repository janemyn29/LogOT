using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;

public class GetLeaveLogRequest : IRequest<PaginatedList<LeaveLogViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetLeaveLogRequestHandler : IRequestHandler<GetLeaveLogRequest, PaginatedList<LeaveLogViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetLeaveLogRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<LeaveLogViewModel>> Handle(GetLeaveLogRequest request, CancellationToken cancellationToken)
    {

        //get LeaveLog 
        
        var LeaveLogs = _applicationDbContext.Get<Domain.Entities.LeaveLog>()
            .Include(x => x.ApplicationUser)
            .Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
       var models = _mapper.ProjectTo<LeaveLogViewModel>(LeaveLogs);
        var page = PaginatedList<LeaveLogViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}