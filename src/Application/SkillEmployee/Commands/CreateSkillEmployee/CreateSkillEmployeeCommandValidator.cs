
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.SkillEmployee.Commands.CreateSkillEmployee;
public class CreateSkillEmployeeCommandValidator : AbstractValidator<CreateSkillEmployeeCommandViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;

    public CreateSkillEmployeeCommandValidator()
    {
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.Level)
            .NotEmpty().WithMessage("AcceptanceType không được để trống.").LessThan(3)
            .WithMessage("AcceptanceType less than or equal 2").GreaterThan(0)
            .WithMessage("AcceptanceType greater than or equal to 1.");
    }

}
