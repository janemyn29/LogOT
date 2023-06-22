using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Degree.Commands.CreateDegree;
public class CreateDegreeCommadValidator : AbstractValidator<CreateDegreeViewModel>
{
    private readonly IApplicationDbContext _context;
    public CreateDegreeCommadValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ApplicationUserId).NotEmpty().WithMessage("Id người dùng không được để trống");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống.");

        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("Hình ảnh không được để trống.");

        RuleFor(v => v.DegreeType)
            .NotNull().WithMessage("Loại bằng cấp không được để trống.").GreaterThan(0).WithMessage("Loại bằng cấp phải lớn hơn 0")
            .LessThan(3).WithMessage("Loại bằng cấp phải bé hơn 3.");
    }
}
