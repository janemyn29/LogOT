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

namespace mentor_v1.Application.LeaveLog.Queries.GetLeaveLog;
public class GetListLeaveLogNoPG : IRequest<List<LeaveLogViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListLeaveLogNoPGHandler : IRequestHandler<GetListLeaveLogNoPG, List<LeaveLogViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListLeaveLogNoPGHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<List<LeaveLogViewModel>> Handle(GetListLeaveLogNoPG request, CancellationToken cancellationToken)
    {

        //get LeaveLog 
        var LeaveLogs = _applicationDbContext.Get<Domain.Entities.LeaveLog>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<LeaveLogViewModel>(LeaveLogs).ToList();

        return Task.FromResult(models);
    }
}
