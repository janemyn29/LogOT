using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.ShiftConfig.Queries;

public class GetListShiftViewmodel : IRequest<List<ShiftViewModel>>
{
}

// IRequestHandler<request type, return type>
public class GetListShiftViewmodelHandler : IRequestHandler<GetListShiftViewmodel, List<ShiftViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListShiftViewmodelHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<ShiftViewModel>> Handle(GetListShiftViewmodel request, CancellationToken cancellationToken)
    {
        // get categories
        var list =  _context.Get<Domain.Entities.ShiftConfig>().Where(x => x.IsDeleted == false).OrderBy(x => x.StartTime).AsNoTracking();
        var map = _mapper.ProjectTo<ShiftViewModel>(list).ToList();

        return Task.FromResult(map);
    }
}


