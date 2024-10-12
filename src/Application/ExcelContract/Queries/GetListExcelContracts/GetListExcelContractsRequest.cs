using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.ExcelContract.Queries.GetListExcelContacts;
public class GetListExcelContractsRequest : IRequest<PaginatedList<ExcelContractsViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public Guid JobReportId { get; set; }
}

public class GetListExcelContactsRequestHandler : IRequestHandler<GetListExcelContractsRequest, PaginatedList<ExcelContractsViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListExcelContactsRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<ExcelContractsViewModel>> Handle(GetListExcelContractsRequest request, CancellationToken cancellationToken)
    {
        var jobReport = _applicationDbContext.Get<Domain.Entities.JobReport>().Where(x => x.IsDeleted == false && x.Id.Equals(request.JobReportId)).FirstOrDefault();
        if (jobReport == null) throw new NotFoundException("Không tìm thấy JobReportId: " + request.JobReportId);

        var list = _applicationDbContext.Get<Domain.Entities.ExcelContract>()
                   .Include(x => x.JobReport).Where(x => x.IsDeleted == false && x.JobReportId == request.JobReportId).AsNoTracking();

        //List<ExcelContractsViewModel> listViewModel = new List<ExcelContractsViewModel>();

        var map = _mapper.ProjectTo<ExcelContractsViewModel>(list);
        var page = PaginatedList<ExcelContractsViewModel>.CreateAsync(map, request.Page, request.Size);
        return page;
    }
}
