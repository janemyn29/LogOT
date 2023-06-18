using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.DepartmentAllowance.Commands.CreateDepartmentAllowance;

namespace mentor_v1.Application.DepartmentAllowance.Commands.UpdateDepartmentAllowance;
public class UpdateDepartmentAllowanceCommandValidator : AbstractValidator<UpdateDepartmentAllowanceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentAllowanceCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.DepartmentId)
            .NotEmpty().WithMessage("Phòng ban không được để trống.");
        RuleFor(v => v.SubsidizeId)
            .NotEmpty().WithMessage("Trợ cấp phòng ban không được để trống.");
    }



}