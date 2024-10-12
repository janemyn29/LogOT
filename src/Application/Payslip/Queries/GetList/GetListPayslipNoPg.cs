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
public class GetListPayslipNoPg : IRequest<List<Domain.Entities.PaySlip>>
{
}

public class GetListPayslipNoPgHandler : IRequestHandler<GetListPayslipNoPg,List<Domain.Entities.PaySlip>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListPayslipNoPgHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.PaySlip>> Handle(GetListPayslipNoPg request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var item = _applicationDbContext.Get<Domain.Entities.PaySlip>().Include(x => x.EmployeeContract.ApplicationUser).Where(x => x.IsDeleted == false).OrderByDescending(x => x.ToTime).AsNoTracking().ToList();

        return Task.FromResult(item);
    }
}