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

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
public class GetListEmpContractRequest : IRequest<PaginatedList<Domain.Entities.EmployeeContract>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListEmpContractRequestHandler : IRequestHandler<GetListEmpContractRequest, PaginatedList<Domain.Entities.EmployeeContract>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListEmpContractRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<Domain.Entities.EmployeeContract>> Handle(GetListEmpContractRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var ListCity = _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.IsDeleted == false).AsNoTracking();

        // map IQueryable<BlogCity> to IQueryable<CityViewModel>
        // AsNoTracking to remove default tracking on entity framework
        // Paginate data
        var page = await PaginatedList<Domain.Entities.EmployeeContract>
            .CreateAsync(ListCity, request.Page, request.Size);

        return page;
    }
}

