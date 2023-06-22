using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.SkillEmployee.Commands.UpdateSkillEmployee;

namespace mentor_v1.Application.SkillEmployee.Commands.UpdateSkillEmployeeCommand;
public class UpdateSkillEmployeeCommand : IRequest
{
    public UpdateSkillEmployeeCommandViewModel updateSkillEmployeeCommandView;
}

public class UpdateSkillEmployeeCommandHandler : IRequestHandler<UpdateSkillEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateSkillEmployeeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UpdateSkillEmployeeCommand request, CancellationToken cancellationToken)
    {
        var skill = await _context.Get<Domain.Entities.SkillEmployee>().FindAsync(new object[] { request.updateSkillEmployeeCommandView.Id }, cancellationToken);
        
        if (skill == null || skill.IsDeleted == true)
        {
            throw new NotFoundException("Không tìm thấy ID " + request.updateSkillEmployeeCommandView.Id);
        
        }
        var mapSkill = _mapper.Map(request.updateSkillEmployeeCommandView, skill);

        if (await _context.SaveChangesAsync(cancellationToken) == 0) throw new Exception();

        return Unit.Value;
    }
}
