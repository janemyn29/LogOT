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
public class GetDependantByUserId : IRequest<PaginatedList<GetDependentViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string userId { get; set; }


}

public class GetDependantByUserIdHandler : IRequestHandler<GetDependantByUserId, PaginatedList<GetDependentViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDependantByUserIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<GetDependentViewModel>> Handle(GetDependantByUserId request, CancellationToken cancellationToken)
    {
        var dependent = _context.Get<Domain.Entities.Dependent>().Where(x => x.IsDeleted == false && x.ApplicationUserId.ToLower().Equals(request.userId.ToLower())).Include(x => x.ApplicationUser).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetDependentViewModel>(dependent);
        var page = PaginatedList<GetDependentViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}

