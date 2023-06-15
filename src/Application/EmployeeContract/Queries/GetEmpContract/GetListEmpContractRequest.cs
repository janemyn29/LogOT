using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Department.Queries.GetDepartment;
using mentor_v1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
public class GetListEmpContractRequest : IRequest<PaginatedList<EmpContractViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListEmpContractRequestHandler : IRequestHandler<GetListEmpContractRequest, PaginatedList<EmpContractViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListEmpContractRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EmpContractViewModel>> Handle(GetListEmpContractRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var ListCity = _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.IsDeleted == false).AsNoTracking();

        // map IQueryable<BlogCity> to IQueryable<CityViewModel>
        var map = _mapper.ProjectTo<EmpContractViewModel>(ListCity);
        // AsNoTracking to remove default tracking on entity framework
        // Paginate data
        var page = await PaginatedList<EmpContractViewModel>
            .CreateAsync(map, request.Page, request.Size);

        return page;
    }
}

