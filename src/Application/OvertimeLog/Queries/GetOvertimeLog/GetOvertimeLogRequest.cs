

using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;

public class GetOvertimeLogRequest : IRequest<PaginatedList<Domain.Entities.OvertimeLog>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetOvertimeLogRequestHandler : IRequestHandler<GetOvertimeLogRequest, PaginatedList<Domain.Entities.OvertimeLog>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetOvertimeLogRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.OvertimeLog>> Handle(GetOvertimeLogRequest request, CancellationToken cancellationToken)
    {

        //get OvertimeLog 
        var OvertimeLogs = _applicationDbContext.Get<Domain.Entities.OvertimeLog>()
            .Include(a => a.ApplicationUser)
            .Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        //var models = _mapper.ProjectTo<OvertimeLogViewModel>(OvertimeLogs);

        var page = PaginatedList<Domain.Entities.OvertimeLog>.CreateAsync(OvertimeLogs, request.Page, request.Size);

        return page;
    }
}