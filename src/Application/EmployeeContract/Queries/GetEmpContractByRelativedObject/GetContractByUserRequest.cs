using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
public class GetContractByUserRequest : IRequest<List<Domain.Entities.EmployeeContract>>
{
    public string UserId { get; set; }


}

// IRequestHandler<request type, return type>
public class GetContractByUserRequestHandler : IRequestHandler<GetContractByUserRequest, List<Domain.Entities.EmployeeContract>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetContractByUserRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.EmployeeContract>> Handle(GetContractByUserRequest request, CancellationToken cancellationToken)
    {

        var EmpContract = _context.Get<Domain.Entities.EmployeeContract>().Include(x=>x.AllowanceEmployees.Where(c=>c.IsDeleted==false)).ThenInclude(x=>x.Allowance).Where(x => x.IsDeleted == false && x.ApplicationUserId.Equals(request.UserId)).AsNoTracking().ToList();
        return Task.FromResult(EmpContract);
    }
}

