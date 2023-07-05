using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Exchange.Queries;
public class GetListExchangeRequest : IRequest<List<Domain.Entities.Exchange>>
{
}

// IRequestHandler<request type, return type>
public class GetListExchangeRequestHandler : IRequestHandler<GetListExchangeRequest, List<Domain.Entities.Exchange>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    // DI
    public GetListExchangeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.Exchange>> Handle(GetListExchangeRequest request, CancellationToken cancellationToken)
    {
        // get categories
        var EmpContract = _context.Get<Domain.Entities.Exchange>().Where(x => x.IsDeleted == false).OrderBy(x => x.Muc_Quy_Doi_From).AsNoTracking().ToList();
        return Task.FromResult(EmpContract); //Task.CompletedTask;
    }
}


