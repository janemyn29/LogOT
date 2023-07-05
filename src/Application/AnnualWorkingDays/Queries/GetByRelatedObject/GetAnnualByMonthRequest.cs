using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.AnnualWorkingDays.Queries.GetList;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;

namespace mentor_v1.Application.AnnualWorkingDays.Queries.GetByRelatedObject;
public class GetAnnualByMonthRequest : IRequest<List<AnnualViewModel>>
{
    public int Month { get; set; }
    public int Year { get; set; }


}
public class GetAnnualByMonthRequestHandler : IRequestHandler<GetAnnualByMonthRequest, List<AnnualViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetAnnualByMonthRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<AnnualViewModel>> Handle(GetAnnualByMonthRequest request, CancellationToken cancellationToken)
    {

        var EmpContract = _context.Get<Domain.Entities.AnnualWorkingDay>().Where(x => x.IsDeleted == false && x.Day.Month == request.Month && x.Day.Year == request.Year).OrderBy(x=>x.Day);
        var map = _mapper.ProjectTo<AnnualViewModel>(EmpContract).ToList();
        return Task.FromResult(map);
    }
}
