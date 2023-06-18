using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Coefficients.Queries.GetCoefficients;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
public class GetListAnnualRequest : IRequest<PaginatedList<AnnualViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListAnnualRequestHandler : IRequestHandler<GetListAnnualRequest, PaginatedList<AnnualViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListAnnualRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AnnualViewModel>> Handle(GetListAnnualRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var ListCity = _context.Get<Domain.Entities.AnnualWorkingDay>().Include(x => x.Coefficient).Where(x => x.IsDeleted == false).OrderByDescending(x=>x.Day).AsNoTracking();

        // map IQueryable<BlogCity> to IQueryable<CityViewModel>
        var map = _mapper.ProjectTo<AnnualViewModel>(ListCity);
        // AsNoTracking to remove default tracking on entity framework
        // Paginate data
        var page = await PaginatedList<AnnualViewModel>
            .CreateAsync(map, request.Page, request.Size);

        return page;
    }
}

