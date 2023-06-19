using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Experience.Commands.CreateExperience;

namespace mentor_v1.Application.Experience.Commands.CreateExperience;

public class CreateExperienceCommandValidator : AbstractValidator<CreateExperienceCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateExperienceCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.ApplicationUserId)
            .NotEmpty().WithMessage("Nhân viên không được để trống.");
        RuleFor(v => v.NameProject)
            .NotEmpty().WithMessage("Tên dự án không được để trống.");
        RuleFor(v => v.TeamSize)
            .NotEmpty().WithMessage("Team size không được để trống.");
        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");
        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("Ngày kết thúc không được để trống.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống.");
        RuleFor(v => v.TechStack)
            .NotEmpty().WithMessage("Công nghệ không được để trống.");
        RuleFor(v => v)
            .Must(v => v.StartDate < v.EndDate)
            .WithMessage("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");
    }



}
