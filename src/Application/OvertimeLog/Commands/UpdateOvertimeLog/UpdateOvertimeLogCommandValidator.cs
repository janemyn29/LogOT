using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;

namespace mentor_v1.Application.OvertimeLog.Commands.UpdateOvertimeLog;

public class UpdateOvertimeLogCommandValidator : AbstractValidator<UpdateOvertimeLogViewModel>
{
    private readonly IApplicationDbContext _context;
    public UpdateOvertimeLogCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.Hours)
            .NotEmpty().WithMessage("Thời gian yêu cầu tăng ca không thể bỏ trống")
            .LessThanOrEqualTo(24).WithMessage("Thời gian yêu cầu tăng ca không vượt quá 24h.");
            
        // Add validation for request
        /*RuleFor(v => v.)
            .NotEmpty().WithMessage("Mô tả không thể để trống.")
            .MaximumLength(200).WithMessage("Mô tả không được quá 200 ký tự.");*/
        // Add validation for request
        //RuleFor(v => v.OvertimeLogViewModel.Positions)
        //    .NotEmpty().WithMessage("Vị trí không được để trống.");
    }
}
