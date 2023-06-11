using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Allowance.Commands.CreateAllowance;
public class CreateAllowanceCommandValidator : AbstractValidator<CreateAllowanceViewModel>
{
    private readonly IApplicationDbContext _context;
    public CreateAllowanceCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.AllowanceType)
            .NotEmpty().WithMessage("Loại phụ cấp không được để trống.").LessThan(3).WithMessage("AllowanceType less than or equal 2").GreaterThan(0).WithMessage("AllowanceType greater than or equal to 1.");

        RuleFor(v => v.Amount).NotEmpty()
            .WithMessage("Tiền không được để trống.");

        RuleFor(v => v.Eligibility_Criteria).NotEmpty()
            .WithMessage("Đủ tiêu chuẩn không được để trống.");

        RuleFor(v => v.Requirements).NotEmpty()
            .WithMessage("Yêu cầu không được để trống.");
    }
}
