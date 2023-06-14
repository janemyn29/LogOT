using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Level.Queries.GetLevel;

namespace mentor_v1.Application.Level.Commands.CreateLevel;

public class CreateLevelCommandValidator : AbstractValidator<LevelViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateLevelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tên cấp độ không thể để trống.")
            .MaximumLength(200).WithMessage("Tên cấp độ không vượt quá 200 kí tự.")
            .MustAsync(BeUniqueName).WithMessage("Tên cấp độ đã tồn tại.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Miêu tả không thể để trống.")
            .MaximumLength(200).WithMessage("Miêu tả không vượt quá 200 ký tự.");
    }

    // Custom action to check with the database
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Get<Domain.Entities.Level>()
            .AllAsync(l => l.Name != name || l.IsDeleted == true, cancellationToken);
    }
}
