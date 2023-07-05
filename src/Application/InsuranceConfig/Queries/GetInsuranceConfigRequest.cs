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

namespace mentor_v1.Application.InsuranceConfig.Queries;
public class GetInsuranceConfigRequest : IRequest<Domain.Entities.InsuranceConfig>
{
}

public class GetInsuranceConfigRequestHandler : IRequestHandler<GetInsuranceConfigRequest, Domain.Entities.InsuranceConfig>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetInsuranceConfigRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<Domain.Entities.InsuranceConfig> Handle(GetInsuranceConfigRequest request, CancellationToken cancellationToken)
    {

        //get Level by ?
        var item = _applicationDbContext.Get<Domain.Entities.InsuranceConfig>().Where(x => x.IsDeleted == false).AsNoTracking().FirstOrDefault();
       

        return Task.FromResult(item);
    }
}
