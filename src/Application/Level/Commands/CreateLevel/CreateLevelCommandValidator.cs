using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;

namespace mentor_v1.Application.Level.Commands.CreateLevel;

public class CreateLevelCommandValidator : AbstractValidator<CreateLevelCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateLevelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            // Can continue with multi chain
            //.MustAsync(BeUniqueName).WithMessage("The specified category name already exists.")
            //.MustAsync(BeUniqueName).WithMessage("The specified category name already exists.")
            .MustAsync(BeUniqueName).WithMessage("The specified Title already exists.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");
        // Can continue with multi chain
        //.MustAsync(BeUniqueName).WithMessage("The specified category name already exists.")
        //.MustAsync(BeUniqueName).WithMessage("The specified category name already exists.")
        //.MustAsync(BeUniqueDescription).WithMessage("Title English Title_English already exists.");
        /*RuleFor(v => v.Positions)
            .NotEmpty().WithMessage("Posotions is required.")
            *//*.MaximumLength(200).WithMessage("Describle must not exceed 200 characters.")*//*
        // Can continue with multi chain
        //.MustAsync(BeUniqueName).WithMessage("The specified category name already exists.")
        //.MustAsync(BeUniqueName).WithMessage("The specified category name already exists.")
            .MustAsync(BeUniquePositions).WithMessage("The specified Describle already exists.");*/

    }

    // Custom action to check with the database
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Get<Domain.Entities.Level>()
            .AllAsync(l => l.Name != name || l.IsDeleted == true, cancellationToken);
    }
    /*public async Task<bool> BeUnique(string name, CancellationToken cancellationToken)
    {
        return await _context.Get<Domain.Entities.Level>()
            .AllAsync(l => l.Positions != name || l.IsDeleted == true, cancellationToken);
    }*/

}
