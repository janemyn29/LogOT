using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Subsidize.Commands.CreateSubsidize;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.Subsidize.Commands.CreateSubsidize;


public class CreateSubsidizeCommandValidator : AbstractValidator<CreateSubsidizeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateSubsidizeCommandValidator(IApplicationDbContext context)
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
