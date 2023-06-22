
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.MaternityEmployee.Commands.CreateMaternityEmployee;
public class CreateMaternityEmployeeCommand : IRequest<Guid>
{
    public CreateMaternityEmployeeViewModel createMaternityEmployeeViewModel;
}

public class CreateMaternityEmployeeCommandHandler : IRequestHandler<CreateMaternityEmployeeCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public CreateMaternityEmployeeCommandHandler(IApplicationDbContext context, IMapper mapper, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateMaternityEmployeeCommand request, CancellationToken cancellationToken)
    {
        var maternity = _mapper.Map<Domain.Entities.MaternityEmployee>(request.createMaternityEmployeeViewModel);

        //check applicationUserID
        if (await _userManager.Users.Where(x => x.Id.Equals(request.createMaternityEmployeeViewModel.ApplicationUserId)).AsNoTracking().FirstOrDefaultAsync() == null)
            throw new ArgumentNullException();

        _context.Get<Domain.Entities.MaternityEmployee>().Add(maternity);

        if (await _context.SaveChangesAsync(cancellationToken) == 0)
            throw new Exception();

        return maternity.Id;
    }
}
