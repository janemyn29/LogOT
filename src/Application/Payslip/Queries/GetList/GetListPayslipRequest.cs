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
public class GetListPayslipRequest : IRequest<PaginatedList<Domain.Entities.PaySlip>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetListPayslipRequestHandler : IRequestHandler<GetListPayslipRequest, PaginatedList<Domain.Entities.PaySlip>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListPayslipRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.PaySlip>> Handle(GetListPayslipRequest request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var item = _applicationDbContext.Get<Domain.Entities.PaySlip>().Include(x => x.EmployeeContract.ApplicationUser).Where(x => x.IsDeleted == false).OrderByDescending(x => x.ToTime).AsNoTracking();

        var page = PaginatedList<Domain.Entities.PaySlip>.CreateAsync(item, request.Page, request.Size);

        return page;
    }
}
