using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Dependent.Queries;
public class GetListDependantNoVmRequest : IRequest<List<GetDependentViewModel>>
{
    public AcceptanceType AcceptanceType { get; set; }

}

public class GetListDependantNoVmRequestHandler : IRequestHandler<GetListDependantNoVmRequest, List<GetDependentViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetListDependantNoVmRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<GetDependentViewModel>> Handle(GetListDependantNoVmRequest request, CancellationToken cancellationToken)
    {
        var dependent = _context.Get<Domain.Entities.Dependent>().Where(x => x.IsDeleted == false &&  x.AcceptanceType == request.AcceptanceType).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetDependentViewModel>(dependent).ToList();
        return Task.FromResult(model);
    }
}
