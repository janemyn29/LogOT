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

namespace mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
public class GetListAnnualFromCurrent : IRequest<List<Domain.Entities.AnnualWorkingDay>>
{
}

// IRequestHandler<request type, return type>
public class GetListAnnualFromCurrentHandler : IRequestHandler<GetListAnnualFromCurrent, List<Domain.Entities.AnnualWorkingDay>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListAnnualFromCurrentHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Domain.Entities.AnnualWorkingDay>> Handle(GetListAnnualFromCurrent request, CancellationToken cancellationToken)
    {
        // get categories
        var ListCity = _context.Get<Domain.Entities.AnnualWorkingDay>().Where(x => x.IsDeleted == false && x.Day.Date > DateTime.Now.Date).OrderByDescending(x => x.Day).AsNoTracking().ToList();

        return ListCity;
    }
}


