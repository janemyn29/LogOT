using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Dependent.Queries;
public class GetDependantByUserIdRequest : IRequest<List<Domain.Entities.Dependent>>
{
    public string UserId { get; set; }
}

public class GetDependantByUserIdRequestHandler : IRequestHandler<GetDependantByUserIdRequest, List<Domain.Entities.Dependent>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDependantByUserIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Domain.Entities.Dependent>> Handle(GetDependantByUserIdRequest request, CancellationToken cancellationToken)
    {
        var dependent = _context.Get<Domain.Entities.Dependent>().Where(x => x.IsDeleted == false && x.ApplicationUserId == request.UserId ).Include(x => x.ApplicationUser).OrderBy(x => x.BirthDate).AsNoTracking().ToList();
        return Task.FromResult(dependent);
    }
}

