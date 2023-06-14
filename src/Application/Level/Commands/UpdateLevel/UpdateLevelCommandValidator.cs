using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Commands.UpdateLevel;

namespace mentor_v1.Application.Level.Commands.UpdateLevel;

public class UpdateLevelCommandValidator : AbstractValidator<UpdateLevelCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateLevelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.LevelViewModel.Name)
            .NotEmpty().WithMessage("Tên cấp độ không thể để trống.")
            .MaximumLength(70).WithMessage("Tên cấp độ không được quá 70 ký tự.");
        // Add validation for request
        RuleFor(v => v.LevelViewModel.Description)
            .NotEmpty().WithMessage("Mô tả không thể để trống.")
            .MaximumLength(200).WithMessage("Mô tả không được quá 200 ký tự.");
        // Add validation for request
        //RuleFor(v => v.LevelViewModel.Positions)
        //    .NotEmpty().WithMessage("Vị trí không được để trống.");
    }
}

