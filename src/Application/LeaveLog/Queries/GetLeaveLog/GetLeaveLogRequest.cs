using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;

public class GetLeaveLogRequest : IRequest<PaginatedList<Domain.Entities.LeaveLog>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetLeaveLogRequestHandler : IRequestHandler<GetLeaveLogRequest, PaginatedList<Domain.Entities.LeaveLog>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetLeaveLogRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.LeaveLog>> Handle(GetLeaveLogRequest request, CancellationToken cancellationToken)
    {

        //get LeaveLog 
        var LeaveLogs = _applicationDbContext.Get<Domain.Entities.LeaveLog>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        //var models = _mapper.ProjectTo<LeaveLogViewModel>(LeaveLogs);

        var page = PaginatedList<Domain.Entities.LeaveLog>.CreateAsync(LeaveLogs, request.Page, request.Size);

        return page;
    }
}