using System;
using System.Collections.Generic;
using System.Data.Entity;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.JobReport.Queries.GetJobReport;
public class GetJobReportByIdRequest : IRequest<JobReportViewModel>
{
    public Guid Id { get; set; }
}

public class GetJobReportByIdRequestHandler : IRequestHandler<GetJobReportByIdRequest, JobReportViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetJobReportByIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<JobReportViewModel> Handle(GetJobReportByIdRequest request, CancellationToken cancellationToken)
    {
        var jobReport = _applicationDbContext.Get<Domain.Entities.JobReport>().Include(x => x.ExcelContracts)
                                                    .FirstOrDefault(x => x.Id.Equals(request.Id) && x.IsDeleted == false);

        

        if(jobReport == null) throw new NotFoundException("Không tìm thấy ID " + request.Id);

        var map = _mapper.Map<JobReportViewModel>(jobReport);

        return Task.FromResult(map);
    }
}
