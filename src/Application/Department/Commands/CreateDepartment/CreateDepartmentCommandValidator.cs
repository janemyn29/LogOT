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
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")         
            .MustAsync(BeUniqueName).WithMessage("The specified Title already exists.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");      
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Get<Domain.Entities.Department>()
            .AllAsync(l => l.Name != name || l.IsDeleted == true, cancellationToken);
    }
   

}

