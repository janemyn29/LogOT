using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;

namespace mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;

public class CreateLeaveLogCommandValidator : AbstractValidator<CreateLeaveLogViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateLeaveLogCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.LeaveDate)
            .NotEmpty().WithMessage("Ngày nghỉ không thể để trống.")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Ngày bắt đầu phải sau ngày hiện tại");
        //.LessThanOrEqualTo(v => v.EndDate).WithMessage("Ngày bắt đầu không thể sau ngày kết thúc");
        //.MustAsync(BeUniqueName).WithMessage("Tên cấp độ đã tồn tại.");

        //RuleFor(v => v.EndDate)
        //    .NotEmpty().WithMessage("Ngày kết thúc không thể để trống.")
        //    .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Ngày kết thúc phải sau ngày hiện tại");

        RuleFor(v => v.LeaveShift)
            .NotEmpty().WithMessage("Ca nghỉ không thể để trống.");
            //.LessThanOrEqualTo(24).WithMessage("Thời lượng giờ nghỉ không thể vượt quá 24h");

        RuleFor(v => v.Reason)
            .NotEmpty().WithMessage("Lý do nghỉ không thể để trống")
            .MaximumLength(1000).WithMessage("Lí do không thể vượt quá 1000 ký tự");
    }

    // Custom action to check with the database
    /*public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var result = await _context.Get<Domain.Entities.LeaveLog>().Where(u => u.Name == name || u.IsDeleted == true).FirstOrDefaultAsync();
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