using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Attendance.Queries.GetAttendance;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Payslip.Queries.GetList;
public class GetListPayslipByContractIdRequest : IRequest<PaginatedList<Domain.Entities.PaySlip>>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public Guid ContractId { get; set; }


}

public class GetListPayslipByContractIdRequestHandler : IRequestHandler<GetListPayslipByContractIdRequest, PaginatedList<Domain.Entities.PaySlip>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetListPayslipByContractIdRequestHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.PaySlip>> Handle(GetListPayslipByContractIdRequest request, CancellationToken cancellationToken)
    {

        //get Attendance by ?
        var item = _applicationDbContext.Get<Domain.Entities.PaySlip>().Include(x => x.EmployeeContract).Where(x => x.IsDeleted == false && x.EmployeeContractId == request.ContractId).OrderByDescending(x => x.ToTime).AsNoTracking();

        var page = PaginatedList<Domain.Entities.PaySlip>.CreateAsync(item, request.Page, request.Size);

        return page;
    }
}