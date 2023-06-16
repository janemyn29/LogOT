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

namespace mentor_v1.Application.ConfigDays.Queries.GetConfigDay;

public class GetListConfigDayRequest : IRequest<PaginatedList<ConfigDayViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

// IRequestHandler<request type, return type>
public class GetListConfigDayRequestHandler : IRequestHandler<GetListConfigDayRequest, PaginatedList<ConfigDayViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListConfigDayRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ConfigDayViewModel>> Handle(GetListConfigDayRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var ListCity = _context.Get<Domain.Entities.ConfigDay>().Where(x => x.IsDeleted == false).AsNoTracking();

        // map IQueryable<BlogCity> to IQueryable<CityViewModel>
        var map = _mapper.ProjectTo<ConfigDayViewModel>(ListCity);
        // AsNoTracking to remove default tracking on entity framework
        // Paginate data
        var page = await PaginatedList<ConfigDayViewModel>
            .CreateAsync(map, request.Page, request.Size);

        return page;
    }
}

