using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Subsidize.Commands.UpdateSubsidize;

namespace mentor_v1.Application.Subsidize.Commands.UpdateSubsidize;


public class UpdateSubsidizeCommandValidator : AbstractValidator<UpdateSubsidizeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSubsidizeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên trợ cấp không được để trống.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả trợ cấp không được để trống.");
        RuleFor(v => v.Amount)
            .NotNull().WithMessage("Tiền trợ cấp không được để trống.")
            .GreaterThan(-1).WithMessage("Tiền trợ cấp phải lớn hơn hoặc bằng 0.");

    }


}