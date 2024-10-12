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

namespace mentor_v1.Application.ShiftConfig.Queries;
public class GetListShiftRequest : IRequest<List<Domain.Entities.ShiftConfig>>
{
}

// IRequestHandler<request type, return type>
public class GetListShiftRequestHandler : IRequestHandler<GetListShiftRequest,List<Domain.Entities.ShiftConfig>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListShiftRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Domain.Entities.ShiftConfig>> Handle(GetListShiftRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var list = await _context.Get<Domain.Entities.ShiftConfig>().Where(x => x.IsDeleted == false).OrderBy(x=>x.StartTime).AsNoTracking().ToListAsync();

        return list ;
    }
}

