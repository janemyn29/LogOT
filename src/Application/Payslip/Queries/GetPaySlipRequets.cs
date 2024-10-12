using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Payslip.Queries;
public class GetPaySlipRequets : IRequest<Domain.Entities.PaySlip>
{
    public Guid Id { get; set; }
}

public class GetPaySlipRequetsHandler : IRequestHandler<GetPaySlipRequets, Domain.Entities.PaySlip>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetPaySlipRequetsHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<Domain.Entities.PaySlip> Handle(GetPaySlipRequets request, CancellationToken cancellationToken)
    {
        //get Attendance by ?
        var item = _applicationDbContext.Get<Domain.Entities.PaySlip>().Include(x => x.EmployeeContract.ApplicationUser).Include(x=>x.DetailTaxes.OrderBy(x=>x.Muc_chiu_thue_From)).Where(x => x.IsDeleted == false && x.Id== request.Id).AsNoTracking().FirstOrDefault();
        return Task.FromResult(item);
    }
}

