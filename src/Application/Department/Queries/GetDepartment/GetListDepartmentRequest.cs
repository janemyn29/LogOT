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

namespace mentor_v1.Application.Department.Queries.GetDepartment;

public class GetListDepartmentRequest : IRequest<PaginatedList<DepartmentViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListDepartmentRequestHandler : IRequestHandler<GetListDepartmentRequest, PaginatedList<DepartmentViewModel>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListDepartmentRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<DepartmentViewModel>> Handle(GetListDepartmentRequest request, CancellationToken cancellationToken)
    {

        //get Department by ?
        var Departments = _applicationDbContext.Get<Domain.Entities.Department>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var models = _mapper.ProjectTo<DepartmentViewModel>(Departments);

        var page = PaginatedList<DepartmentViewModel>.CreateAsync(models, request.Page, request.Size);

        return page;
    }
}