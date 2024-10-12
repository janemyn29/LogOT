using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public record UpdateUnlockAccount : IRequest
{
    public string id { get; set; }
}

public class UpdateUnlockAccountHandler : IRequestHandler<UpdateUnlockAccount>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateUnlockAccountHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateUnlockAccount request, CancellationToken cancellationToken)
    {

        var otherUser = _userManager.Users.Where(x => x.Id.ToLower().Equals(request.id.ToLower())).FirstOrDefault();
        if (otherUser == null)
        {
            throw new InvalidOperationException("Không tìm thấy người dùng bạn yêu cầu!");
        }

        otherUser.WorkStatus = Domain.Enums.WorkStatus.StillWork;
        otherUser.LockoutEnabled = true;
        otherUser.LockoutEnd = null;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

