using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public record UpdateEmailCommand : IRequest
{
    public UpdateMailModel model { get; set; }
}

public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateEmailCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var CurrentUser = _userManager.FindByIdAsync(request.model.UserId).Result;
        var otherUser = _userManager.Users.Where(x => !x.Id.ToLower().Equals(request.model.UserId.ToLower()) && x.Email.ToLower().Trim().Equals(request.model.NewEmail.ToLower().Trim())).FirstOrDefault();
        if (otherUser != null)
        {
            throw new InvalidOperationException("Email đã được sử dụng cho người khác!");
        }
        if (CurrentUser == null)
        {
            throw new NotFoundException("Không tìm thấy người dùng bạn yêu cầu!");
        }
        CurrentUser.Email = request.model.NewEmail;
        CurrentUser.NormalizedEmail = request.model.NewEmail.ToUpper();
        CurrentUser.EmailConfirmed = false;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

