using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Attendance.Commands.CreateAttendance;

namespace mentor_v1.Application.Attendance.Commands.CreateAttendance;

public class CreateAttendanceCommandValidator : AbstractValidator<CreateAttendanceCommand>
{
   /* private readonly IApplicationDbContext _context;

    public CreateAttendanceCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.ApplicationUserId)
            .NotEmpty().WithMessage("Nhân viên không được để trống.");
        RuleFor(v => v.Day)
            .NotEmpty().WithMessage("Ngày không được để trống.");
        RuleFor(v => v.StartTime)
            .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");
        RuleFor(v => v.EndTime)
            .NotEmpty().WithMessage("Ngày kết thúc không được để trống.");
        RuleFor(v => v.ShiftEnum)
            .NotNull().WithMessage("Ca làm không được để trống.");
        RuleFor(v => v)
            .Must(v => v.StartTime < v.EndTime)
            .WithMessage("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.")
            .Must(v => v.Day < v.EndTime)
            .WithMessage("Ngày phải nhỏ hơn ngày kết thúc.");
    }*/



}
