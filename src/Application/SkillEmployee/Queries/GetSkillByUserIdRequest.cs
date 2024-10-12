using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;
using mentor_v1.Application.SkillEmployee.Queries.GetSkillEmployee;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.SkillEmployee.Queries;
public class GetSkillByUserIdRequest : IRequest<PaginatedList<GetSkillEmployeeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string UserId { get; set; }
}

public class GetSkillByUserIdRequestHandler : IRequestHandler<GetSkillByUserIdRequest, PaginatedList<GetSkillEmployeeViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSkillByUserIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<PaginatedList<GetSkillEmployeeViewModel>> Handle(GetSkillByUserIdRequest request, CancellationToken cancellationToken)
    {
        var skill = _context.Get<Domain.Entities.SkillEmployee>().Where(x => x.IsDeleted == false && x.ApplicationUserId.ToLower().Equals(request.UserId.ToLower())).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetSkillEmployeeViewModel>(skill);
        var page = PaginatedList<GetSkillEmployeeViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}
