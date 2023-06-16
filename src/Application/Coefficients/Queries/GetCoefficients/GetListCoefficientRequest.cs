using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Coefficients.Queries.GetCoefficients;
public class GetListCoefficientRequest : IRequest<PaginatedList<CoefficientViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListCoefficientRequestHandler : IRequestHandler<GetListCoefficientRequest, PaginatedList<CoefficientViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListCoefficientRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CoefficientViewModel>> Handle(GetListCoefficientRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var ListCity = _context.Get<Domain.Entities.Coefficient>().Where(x => x.IsDeleted == false).AsNoTracking();

        // map IQueryable<BlogCity> to IQueryable<CityViewModel>
        var map = _mapper.ProjectTo<CoefficientViewModel>(ListCity);
        // AsNoTracking to remove default tracking on entity framework
        // Paginate data
        var page = await PaginatedList<CoefficientViewModel>
            .CreateAsync(map, request.Page, request.Size);

        return page;
    }
}
