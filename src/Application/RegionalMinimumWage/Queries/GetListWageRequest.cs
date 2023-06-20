using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Coefficients.Queries.GetCoefficients;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.RegionalMinimumWage.Queries;
public class GetListWageRequest : IRequest<List<RegionalMinimumWageViewModel>>
{
}

// IRequestHandler<request type, return type>
public class GetListWageRequestHandler : IRequestHandler<GetListWageRequest, List<RegionalMinimumWageViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListWageRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<List<RegionalMinimumWageViewModel>> Handle(GetListWageRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var list = _context.Get<Domain.Entities.RegionalMinimumWage>().Where(x => x.IsDeleted == false).AsNoTracking();

        var map = _mapper.Map<List<RegionalMinimumWageViewModel>>(list);
      

        return Task.FromResult(map);
    }
}

