using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public record UpdateUserWorkStatusRequest : IRequest
{
    public string id { get; set; }
}

public class UpdateUserWorkStatusRequestHandler : IRequestHandler<UpdateUserWorkStatusRequest>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateUserWorkStatusRequestHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateUserWorkStatusRequest request, CancellationToken cancellationToken)
    {
       
        var otherUser = _userManager.Users.Where(x => x.Id.ToLower().Equals(request.id.ToLower())).FirstOrDefault();
        if (otherUser == null)
        {
            throw new InvalidOperationException("Không tìm thấy người dùng bạn yêu cầu!");
        }

        otherUser.WorkStatus = Domain.Enums.WorkStatus.Quit;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

