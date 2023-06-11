using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Degree.Commands.UpdateDegree;
public class UdpateDegreeValidator : AbstractValidator<UpdateDegreeViewModel>
{
    private readonly IApplicationDbContext _context;
    public UdpateDegreeValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống.");

        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("Hình ảnh không được để trống.");

        RuleFor(v => v.DegreeType)
            .NotEmpty().WithMessage("Loại bằng cấp không được để trống.").LessThan(3).WithMessage("DegreeType less than or equal 2").GreaterThan(0).WithMessage("DegreeType greater than or equal to 1.");
    }
}
