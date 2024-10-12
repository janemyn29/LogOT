using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public record UpdateUserCommand : IRequest
{
    public UpdateUserModel model { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateUserCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var CurrentUser = _userManager.FindByNameAsync(request.model.Username).Result;
        var otherUser = _userManager.Users.Where(x=>!x.UserName.Equals(request.model.Username) && x.IdentityNumber.Equals(request.model.IdentityNumber)).FirstOrDefault();
        if(otherUser != null)
        {
            throw new InvalidOperationException("Số cccd đã được sử dụng cho người khác!");
        }
        if (CurrentUser == null)
        {
            throw new NotFoundException("Không tìm thấy người dùng bạn yêu cầu!");
        }
        
        CurrentUser.PositionId = request.model.PositionId;
        CurrentUser.Fullname = request.model.Fullname;
        CurrentUser.Address = request.model.Address;
        CurrentUser.GenderType = request.model.GenderType;
        CurrentUser.IdentityNumber = request.model.IdentityNumber;
        CurrentUser.BirthDay = request.model.BirthDay;
        CurrentUser.BankAccountNumber = request.model.BankAccountNumber;
        CurrentUser.BankAccountName = request.model.BankAccountName;
        CurrentUser.BankName = request.model.BankName;
        CurrentUser.Image= request.model.Image;
        CurrentUser.PhoneNumber = request.model.PhoneNumber;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

