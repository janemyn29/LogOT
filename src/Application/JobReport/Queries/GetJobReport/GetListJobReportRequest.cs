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

namespace mentor_v1.Application.JobReport.Queries.GetJobReport;
public class GetListJobReportRequest : IRequest<PaginatedList<JobReportViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListJobReoportRequestHandler : IRequestHandler<GetListJobReportRequest, PaginatedList<JobReportViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListJobReoportRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<JobReportViewModel>> Handle(GetListJobReportRequest request, CancellationToken cancellationToken)
    {
        var jobReport = _applicationDbContext.Get<Domain.Entities.JobReport>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<JobReportViewModel>(jobReport);
        var page = PaginatedList<JobReportViewModel>.CreateAsync(models, request.Page, request.Size);
        return page;
    }
}
