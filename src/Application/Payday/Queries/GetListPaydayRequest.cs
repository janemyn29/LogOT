using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Payday.Queries;
public class GetListPaydayRequest : IRequest<List<Domain.Entities.PayDay>>
{
}

// IRequestHandler<request type, return type>
public class GetListPaydayRequestHandler : IRequestHandler<GetListPaydayRequest, List<Domain.Entities.PayDay>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListPaydayRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.PayDay>> Handle(GetListPaydayRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.PayDay>().Where(x => x.IsDeleted == false).OrderBy(x => x.PaymentDay).AsNoTracking().ToList();
        return Task.FromResult(EmpContract); //Task.CompletedTask;
    }
}
