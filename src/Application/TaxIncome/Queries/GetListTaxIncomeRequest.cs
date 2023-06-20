using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.TaxIncome.Queries;
public class GetListTaxIncomeRequest : IRequest<List<Domain.Entities.DetailTaxIncome>>
{
}

// IRequestHandler<request type, return type>
public class GetListTaxIncomeRequestHandler : IRequestHandler<GetListTaxIncomeRequest, List<Domain.Entities.DetailTaxIncome>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListTaxIncomeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.DetailTaxIncome>> Handle(GetListTaxIncomeRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.DetailTaxIncome>().Where(x => x.IsDeleted == false).OrderBy(x=>x.Muc_chiu_thue_From).AsNoTracking().ToList();
        return Task.FromResult(EmpContract); //Task.CompletedTask;
    }
}


