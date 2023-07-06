using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Degree.Commands.CreateDegree;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.Dependent.Commands.CreateDependent;
public class CreateDepentdentCommandValidator : AbstractValidator<CreateDependentViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
    public CreateDepentdentCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ApplicationUserId).NotEmpty().WithMessage("Id người dùng không được để trống");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.BirthDate)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.");


        RuleFor(v => v.Desciption)
            .NotEmpty().WithMessage("Mô tả không được để trống.");

        RuleFor(v => v.Relationship)
            .NotEmpty().WithMessage("Mối quan hệ không được để trống.");
    }
}
