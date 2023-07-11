using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public record UpdateMaterityStatus : IRequest
{
    public string id { get; set; }
    public bool IsMaternity { get; set; } = false;
}

public class UpdateMaterityStatusHandler : IRequestHandler<UpdateMaterityStatus>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateMaterityStatusHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateMaterityStatus request, CancellationToken cancellationToken)
    {

        var otherUser = _userManager.Users.Where(x => x.Id.ToLower().Equals(request.id.ToLower())).FirstOrDefault();
        if (otherUser == null)
        {
            throw new InvalidOperationException("Không tìm thấy người dùng bạn yêu cầu!");
        }

        otherUser.IsMaternity = request.IsMaternity;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


