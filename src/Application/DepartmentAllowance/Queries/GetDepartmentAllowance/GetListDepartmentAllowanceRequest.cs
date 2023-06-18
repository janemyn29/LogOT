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
using mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowance;

namespace mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowance;

public class GetListDepartmentAllowanceRequest : IRequest<PaginatedList<DepartmentAllowanceViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListDepartmentAllowanceRequestHandler : IRequestHandler<GetListDepartmentAllowanceRequest, PaginatedList<DepartmentAllowanceViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListDepartmentAllowanceRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<DepartmentAllowanceViewModel>> Handle(GetListDepartmentAllowanceRequest request, CancellationToken cancellationToken)
    {

        //get DepartmentAllowance by ?
        var DepartmentAllowances = _applicationDbContext.Get<Domain.Entities.DepartmentAllowance>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<DepartmentAllowanceViewModel>(DepartmentAllowances);

        var page = PaginatedList<DepartmentAllowanceViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}