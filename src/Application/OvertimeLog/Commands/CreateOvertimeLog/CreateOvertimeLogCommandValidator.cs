using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;

namespace mentor_v1.Application.OvertimeLog.Commands.CreateOvertimeLog;

public class CreateOvertimeLogCommandValidator : AbstractValidator<CreateOvertimeLogViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateOvertimeLogCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        //RuleFor(v => v.ApplicationUserId)
        //    .NotEmpty().WithMessage("Id người dùng không thể để trống.")
        //    .MaximumLength(20).WithMessage("Id không vượt quá 20 kí tự.");
        //.MustAsync(BeUniqueName).WithMessage("Tên cấp độ đã tồn tại.");
        RuleFor(v => v.Hours)
            .NotEmpty().WithMessage("Hãy nhập số giờ tăng ca.")
            .GreaterThan(0).WithMessage("Thời gian tăng ca không thể là số âm")
            .LessThanOrEqualTo(24).WithMessage("Thời gian tăng ca không thể vượt quá 24h");
        /*RuleFor(v => v.CancelReason)
            .MaximumLength(1000).WithMessage("Lí do từ chối không thể vượt quá 1000 ký tự");*/
    }

    // Custom action to check with the database
    /*public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var result = await _context.Get<Domain.Entities.OvertimeLog>().Where(u => u.Name == name || u.IsDeleted == true).FirstOrDefaultAsync();
        if (result == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/
}
