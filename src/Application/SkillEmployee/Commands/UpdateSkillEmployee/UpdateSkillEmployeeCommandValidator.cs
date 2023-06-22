using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.SkillEmployee.Commands.UpdateSkillEmployee;
public class UpdateSkillEmployeeCommandValidator : AbstractValidator<UpdateSkillEmployeeCommandViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public UpdateSkillEmployeeCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id không được để trống.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.Level)
            .NotNull().WithMessage("Cấp độ không được để trống.").LessThan(3)
            .WithMessage("Cấp độ phải bé hơn hoặc bằng 2").GreaterThan(0)
            .WithMessage("Cấp độ phải lớn hơn hoặc bằng 1.");
    }
}
