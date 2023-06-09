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
            .NotEmpty().WithMessage("Name cannot be blank.");

        RuleFor(v => v.AllowanceType)
            .NotEmpty().WithMessage("AllowanceType cannot be blank.").LessThan(2).WithMessage("Less than 2").GreaterThan(-1).WithMessage("Greater than or equal to 0.");

        RuleFor(v => v.Amount).NotEmpty()
            .WithMessage("Amount cannot be blank");

        RuleFor(v => v.Eligibility_Criteria).NotEmpty()
            .WithMessage("Eligibility_Criteria cannot be blank");

        RuleFor(v => v.Requirements).NotEmpty()
            .WithMessage("Requirements cannot be blank");
    }
}
