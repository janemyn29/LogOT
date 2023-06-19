using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.SkillEmployee.Commands.CreateSkillEmployee;
public class CreateSkillEmployeeCommand : IRequest<Guid>
{
    public CreateSkillEmployeeCommandViewModel createSkillEmployeeCommandView;
}

public class CreateSkillEmployeeCommandHandler : IRequestHandler<CreateSkillEmployeeCommand, Guid>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public CreateSkillEmployeeCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<Guid> Handle(CreateSkillEmployeeCommand request, CancellationToken cancellationToken)
    {
        var skill = _mapper.Map<Domain.Entities.SkillEmployee>(request.createSkillEmployeeCommandView);

        //check Id ờ bảng User
        if (await _userManager.Users.Where(x => x.Id.Equals(request.createSkillEmployeeCommandView.ApplicationUserId)).FirstOrDefaultAsync() == null)
            throw new ArgumentNullException();

        _applicationDbContext.Get<Domain.Entities.SkillEmployee>().Add(skill);

        if (await _applicationDbContext.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return skill.Id;
    }
}