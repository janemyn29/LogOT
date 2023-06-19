
using System.Data.Entity;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Models;

namespace mentor_v1.Application.SkillEmployee.Queries.GetSkillEmployee;
public class GetSkillEmployeeRequest : IRequest<PaginatedList<GetSkillEmployeeViewModel>>
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public class GetSkillEmployeeRequestHandler : IRequestHandler<GetSkillEmployeeRequest, PaginatedList<GetSkillEmployeeViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSkillEmployeeRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public  Task<PaginatedList<GetSkillEmployeeViewModel>> Handle(GetSkillEmployeeRequest request, CancellationToken cancellationToken)
    {
        var skill = _context.Get<Domain.Entities.SkillEmployee>().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Created).AsNoTracking();
        var model = _mapper.ProjectTo<GetSkillEmployeeViewModel>(skill);
        var page = PaginatedList<GetSkillEmployeeViewModel>.CreateAsync(model, request.Page, request.Size);
        return page;
    }
}
