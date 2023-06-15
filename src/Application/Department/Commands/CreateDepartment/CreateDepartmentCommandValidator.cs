using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Commands.CreateLevel;

namespace mentor_v1.Application.Department.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên phòng ban không được để trống.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Mô tả phòng ban không được để trống.");
    }

   

}

