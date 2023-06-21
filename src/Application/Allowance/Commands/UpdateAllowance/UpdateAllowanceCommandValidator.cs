
using FluentValidation;
using mentor_v1.Application.Allowance.Commands.UpdateAllowance;
using mentor_v1.Application.Common.Interfaces;


namespace mentor_v1.Application.ApplicationAllowance.Commands.UpdateAllowance;
public class UpdateAllowanceCommandValidator : AbstractValidator<UpdateAllowanceViewModel>
{
    private readonly IApplicationDbContext _context;
    public UpdateAllowanceCommandValidator(IApplicationDbContext context)
    { 
        _context = context;

        RuleFor(v => v.Id).NotEmpty().WithMessage("Id không được để trống.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.AllowanceType)
            .NotNull().WithMessage("Loại phụ cấp không được để trống.").GreaterThan(0).WithMessage("Loại phụ cấp phải lớn hơn 0")
            .LessThan(3).WithMessage("Loại phụ cấp phải bé hơn 3.");

        RuleFor(v => v.Amount).NotNull()
            .WithMessage("Tiền không được để trống.").GreaterThan(-1).WithMessage("Tiền phải lớn hơn hoặc bằng 0.");

        RuleFor(v => v.Eligibility_Criteria).NotEmpty()
            .WithMessage("Đủ tiêu chuẩn không được để trống.");

        RuleFor(v => v.Requirements).NotEmpty()
            .WithMessage("Yêu cầu không được để trống.");
    }
}
