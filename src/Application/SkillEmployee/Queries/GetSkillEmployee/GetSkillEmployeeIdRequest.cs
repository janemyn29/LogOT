using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.SkillEmployee.Queries.GetSkillEmployee;
public class GetSkillEmployeeIdRequest : IRequest<GetSkillEmployeeViewModel>
{
    public string id { get; set; }
}

public class GetSkillEmployeeIdRequestHandler : IRequestHandler<GetSkillEmployeeIdRequest, GetSkillEmployeeViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSkillEmployeeIdRequestHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<GetSkillEmployeeViewModel> Handle(GetSkillEmployeeIdRequest request, CancellationToken cancellationToken)
    {
        var skill = _context.Get<Domain.Entities.SkillEmployee>().Where(x => x.ApplicationUserId.Equals(request.id) && x.IsDeleted == false).FirstOrDefault();
        if (skill == null) throw new NotFoundException("Không tìm thấy kĩ năng của nhân viên này: " + request.id);
        var map = _mapper.Map<GetSkillEmployeeViewModel>(skill);
        return Task.FromResult(map);
    }
}
