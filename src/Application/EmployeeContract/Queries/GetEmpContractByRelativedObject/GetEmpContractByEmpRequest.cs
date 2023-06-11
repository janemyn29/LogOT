using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Level.Queries.GetLevel;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContractByRelativedObject;
public class GetEmpContractByEmpRequest : IRequest<PaginatedList<Domain.Entities.EmployeeContract>>
{
    public string Username { get; set; }
    public int page { get; set; }
    public int size { get; set; }


}

// IRequestHandler<request type, return type>
public class GetEmpContractByEmpRequestHandler : IRequestHandler<GetEmpContractByEmpRequest, PaginatedList<Domain.Entities.EmployeeContract>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetEmpContractByEmpRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<Domain.Entities.EmployeeContract>> Handle(GetEmpContractByEmpRequest request, CancellationToken cancellationToken)
    {

        var EmpContract = _context.Get<Domain.Entities.EmployeeContract>().Where(x => x.IsDeleted == false && x.ApplicationUser.UserName.Equals(request.Username));
        var page = PaginatedList<Domain.Entities.EmployeeContract>.CreateAsync(EmpContract, request.page, request.size);
        return page; 
    }
}




