
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
        RuleFor(x => x.ApplicationUserId).NotEmpty().WithMessage("Id người dùng không được để trống");

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
