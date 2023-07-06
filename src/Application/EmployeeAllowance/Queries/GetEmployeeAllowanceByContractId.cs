using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.Dependent.Queries;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.EmployeeAllowance.Queries;
public class GetEmployeeAllowanceByContractId : IRequest<List<Domain.Entities.AllowanceEmployee>>
{
    public Guid ContractId { get; set; }
}

public class GetEmployeeAllowanceByContractIdHandler : IRequestHandler<GetEmployeeAllowanceByContractId, List<Domain.Entities.AllowanceEmployee>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeeAllowanceByContractIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.AllowanceEmployee>> Handle(GetEmployeeAllowanceByContractId request, CancellationToken cancellationToken)
    {
        var dependent = _context.Get<Domain.Entities.AllowanceEmployee>().Where(x => x.IsDeleted == false && x.AllowanceId== request.ContractId).Include(x => x.Allowance).OrderByDescending(x => x.Created).AsNoTracking().ToList();
        return Task.FromResult(dependent);
    }
}

