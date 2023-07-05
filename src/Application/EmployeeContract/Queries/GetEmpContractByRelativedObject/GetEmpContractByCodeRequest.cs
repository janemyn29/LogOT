
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
public class GetEmpContractByCodeRequest : IRequest<EmpContractViewModel>
{
    public string code { get; set; }

}

// IRequestHandler<request type, return type>
public class GetEmpContractByCodeRequestHandler : IRequestHandler<GetEmpContractByCodeRequest, EmpContractViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetEmpContractByCodeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<EmpContractViewModel> Handle(GetEmpContractByCodeRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.EmployeeContract>().Include(x=>x.ApplicationUser).Include(x=>x.AllowanceEmployees).Where(x => x.IsDeleted == false && x.ContractCode == request.code).AsNoTracking().FirstOrDefault();
        if (EmpContract == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng mà bạn yêu cầu!");
        }
        // map IQueryable<BlogCity> to IQueryable<GetCity.CityViewModel>
        var map = _mapper.Map<EmpContractViewModel>(EmpContract);
        // AsNoTracking to remove default tracking on entity framework

        // Paginate data
        return Task.FromResult(map); //Task.CompletedTask;
    }
}



