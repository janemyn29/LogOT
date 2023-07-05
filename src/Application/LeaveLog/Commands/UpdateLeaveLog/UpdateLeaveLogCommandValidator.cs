using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;

namespace mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;

public class UpdateLeaveLogCommandValidator : AbstractValidator<UpdateLeaveLogViewModel>
{
    private readonly IApplicationDbContext _context;
    public UpdateLeaveLogCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request

        RuleFor(v => v.StartDate)
           .NotEmpty().WithMessage("Ngày bắt đầu không thể để trống.")
           .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Ngày bắt đầu phải sau ngày hiện tại")
           .LessThanOrEqualTo(v => v.EndDate).WithMessage("Ngày bắt đầu không thể sau ngày kết thúc");
        //.MustAsync(BeUniqueName).WithMessage("Tên cấp độ đã tồn tại.");

        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("Ngày kết thúc không thể để trống.")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Ngày kết thúc phải sau ngày hiện tại");

        RuleFor(v => v.LeaveHours)
            .NotEmpty().WithMessage("Thời lượng yêu cầu nghỉ không thể bỏ trống")
            .LessThanOrEqualTo(24).WithMessage("Thời gian yêu cầu nghỉ không vượt quá 24h.");

        // Add validation for request
        RuleFor(v => v.Reason)
            .NotEmpty().WithMessage("Lý do không thể để trống.")
            .MaximumLength(1000).WithMessage("Mô tả không được quá 1000 ký tự.");
        // Add validation for request
        //RuleFor(v => v.LeaveLogViewModel.Positions)
        //    .NotEmpty().WithMessage("Vị trí không được để trống.");
    }
}