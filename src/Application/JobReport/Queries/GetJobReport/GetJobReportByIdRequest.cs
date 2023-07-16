
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.ExcelContract.Queries.GetListExcelContacts;
using mentor_v1.Application.ExcelEmployeeQuit.Queries.GetListExcelEmployeeQuit;
using mentor_v1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.JobReport.Queries.GetJobReport;
public class GetJobReportByIdRequest : IRequest<GetJobReportByIdViewModel>
{
    public Guid Id { get; set; }
}

public class GetJobReportByIdRequestHandler : IRequestHandler<GetJobReportByIdRequest, GetJobReportByIdViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetJobReportByIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<GetJobReportByIdViewModel> Handle(GetJobReportByIdRequest request, CancellationToken cancellationToken)
    {
        var jobReport = _applicationDbContext.Get<Domain.Entities.JobReport>()
                            .Include(x => x.ExcelContracts.Where(x => x.IsDeleted == false))
                            .Include(x => x.ExcelEmployeeQuits.Where(x => x.IsDeleted == false))
                            .Where(x => x.IsDeleted == false && x.Id == request.Id).AsNoTracking().FirstOrDefault();


        if (jobReport == null) throw new NotFoundException("Không tìm thấy ID " + request.Id);

        var map = _mapper.Map<GetJobReportByIdViewModel>(jobReport);
        return Task.FromResult(map);
    }
}