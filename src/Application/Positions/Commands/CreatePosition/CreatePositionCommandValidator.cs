using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.Positions.Commands.CreatePosition;
public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreatePositionCommandValidator(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;

        // Add validation for request
        RuleFor(v => v.LevelId)
            .NotEmpty().WithMessage("Trình độ không được để trống.");
        // Add validation for request
        RuleFor(v => v.DepartmentId)
            .NotEmpty().WithMessage("Phòng ban không được để trống.");
        // Add validation for request
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên vị trí không được để trống.")
            .MaximumLength(100).WithMessage("Tên vị trí không được để trống.");
       
    }
}