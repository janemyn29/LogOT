using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
public class GetEmpContractByCodeRequest : IRequest<Domain.Entities.EmployeeContract>
{
    public string code { get; set; }

}

// IRequestHandler<request type, return type>
public class GetEmpContractByCodeRequestHandler : IRequestHandler<GetEmpContractByCodeRequest, Domain.Entities.EmployeeContract>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetEmpContractByCodeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Domain.Entities.EmployeeContract> Handle(GetEmpContractByCodeRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.IsDeleted == false && x.ContractCode == request.code).FirstOrDefault();
        if (EmpContract == null)
        {
            throw new NotFoundException("Không tìm thấy hợp đồng mà bạn yêu cầu!");
        }
        // map IQueryable<BlogCity> to IQueryable<GetCity.CityViewModel>
        // AsNoTracking to remove default tracking on entity framework

        // Paginate data
        return Task.FromResult(EmpContract); //Task.CompletedTask;
    }
}



