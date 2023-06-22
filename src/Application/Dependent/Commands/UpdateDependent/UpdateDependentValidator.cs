using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Dependent.Commands.UpdateDependent;
public class UpdateDependentValidator : AbstractValidator<UpdateDependentViewModel>
{
    private readonly IApplicationDbContext _context;
    public UpdateDependentValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id).NotEmpty().WithMessage("Id không được để trống.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.BirthDate)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.");

        RuleFor(v => (DateTime.UtcNow.Year - v.BirthDate.Year)).GreaterThanOrEqualTo(18)
            .WithMessage("Ngày sinh không được nhỏ hơn 18.");

        RuleFor(v => v.Desciption)
            .NotEmpty().WithMessage("Mô tả không được để trống.");

        RuleFor(v => v.Relationship)
            .NotEmpty().WithMessage("Mối quan hệ không được để trống.");

        RuleFor(v => v.AcceptanceType).NotNull().WithMessage("Loại chập nhận không được để trống.").
            GreaterThan(0).WithMessage("Loại chập nhận phải lớn hơn 0").LessThan(4)
            .WithMessage("Loại chập nhận phải bé hơn 4.");
    }
}

