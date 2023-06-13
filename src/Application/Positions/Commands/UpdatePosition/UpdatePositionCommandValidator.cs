using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Positions.Commands.CreatePosition;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.Positions.Commands.UpdatePosition;
public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public UpdatePositionCommandValidator(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
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
        // Add validation for request
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id vị trí công việc không được để trống.");

    }
}