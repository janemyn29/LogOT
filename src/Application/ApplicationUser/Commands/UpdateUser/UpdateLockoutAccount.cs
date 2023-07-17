using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public record UpdateLockoutAccount : IRequest
{
    public string id { get; set; }
}

public class UpdateLockoutAccountHandler : IRequestHandler<UpdateLockoutAccount>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateLockoutAccountHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateLockoutAccount request, CancellationToken cancellationToken)
    {

        var otherUser = _userManager.Users.Where(x => x.Id.ToLower().Equals(request.id.ToLower())).FirstOrDefault();
        if (otherUser == null)
        {
            throw new InvalidOperationException("Không tìm thấy người dùng bạn yêu cầu!");
        }

        otherUser.WorkStatus = Domain.Enums.WorkStatus.Quit;
        otherUser.LockoutEnabled = true;
        otherUser.LockoutEnd = DateTime.Now.AddYears(200);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

