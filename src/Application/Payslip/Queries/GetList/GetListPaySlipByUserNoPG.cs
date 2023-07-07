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

namespace mentor_v1.Application.Payslip.Queries.GetList;
public class GetListPaySlipByUserNoPG : IRequest<List<Domain.Entities.PaySlip>>
{
    public string userId { get; set; }
}

public class GetListPaySlipByUserNoPGHandler : IRequestHandler<GetListPaySlipByUserNoPG, List<Domain.Entities.PaySlip>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListPaySlipByUserNoPGHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.PaySlip>> Handle(GetListPaySlipByUserNoPG request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var item = _applicationDbContext.Get<Domain.Entities.PaySlip>().Include(x => x.EmployeeContract.ApplicationUser).Where(x => x.IsDeleted == false && x.EmployeeContract.ApplicationUser.Id.ToLower().Equals(request.userId.ToLower())).OrderByDescending(x => x.ToTime).AsNoTracking().ToList();
        return Task.FromResult(item);
    }
}
