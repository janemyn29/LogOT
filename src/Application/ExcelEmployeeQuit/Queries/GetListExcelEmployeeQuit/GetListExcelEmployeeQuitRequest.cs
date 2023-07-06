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

namespace mentor_v1.Application.ExcelEmployeeQuit.Queries.GetListExcelEmployeeQuit;
public class GetListExcelEmployeeQuitRequest : IRequest<PaginatedList<ExcelEmployeeQuitViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListExcelEmployeeQuitRequestHandler : IRequestHandler<GetListExcelEmployeeQuitRequest, PaginatedList<ExcelEmployeeQuitViewModel>>
{

    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListExcelEmployeeQuitRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<ExcelEmployeeQuitViewModel>> Handle(GetListExcelEmployeeQuitRequest request, CancellationToken cancellationToken)
    {
        var excelEmployee = _applicationDbContext.Get<Domain.Entities.ExcelEmployeeQuit>().Where(x => x.IsDeleted == false).AsNoTracking();

        var models = _mapper.ProjectTo<ExcelEmployeeQuitViewModel>(excelEmployee);
        var page = PaginatedList<ExcelEmployeeQuitViewModel>.CreateAsync(models, request.Page, request.Size);
        return page;
    }
}
