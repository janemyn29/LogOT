using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.RegionalMinimumWage.Queries;
public class GetRegionalWageByRegionTypeNoVm : IRequest<Domain.Entities.RegionalMinimumWage>
{
    public RegionType RegionType { get; set; }

}

// IRequestHandler<request type, return type>
public class GetRegionalWageByRegionTypeNoVmHandler : IRequestHandler<GetRegionalWageByRegionTypeNoVm, Domain.Entities.RegionalMinimumWage>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetRegionalWageByRegionTypeNoVmHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.RegionalMinimumWage> Handle(GetRegionalWageByRegionTypeNoVm request, CancellationToken cancellationToken)
    {
        // get categories
        var item = _context.Get<Domain.Entities.RegionalMinimumWage>().Where(x => x.IsDeleted == false && x.RegionType == request.RegionType).FirstOrDefault();
        if (item == null)
        {
            throw new NotFoundException("Không tìm thấy mức lương cơ sở của vùng bạn yêu cầu mà bạn yêu cầu!");
        }
        return Task.FromResult(item); //Task.CompletedTask;
    }
}
