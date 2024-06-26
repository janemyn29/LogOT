﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;

namespace mentor_v1.Application.MaternityEmployee.Commands.CreateMaternityEmployee;
public class CreateMaternityEmployeeValidator : AbstractValidator<CreateMaternityEmployeeViewModel>
{
    public CreateMaternityEmployeeValidator()
    { 
        RuleFor(x => x.ApplicationUserId).NotEmpty().WithMessage("Id người dùng không được để trống");

        RuleFor(x => x.Image).NotEmpty().WithMessage("Hình ảnh không được để trống");

        RuleFor(v => v.BirthDay).NotEmpty().WithMessage("Ngày sinh không được để trống.");

        RuleFor(v => v.AcceptanceType).NotNull().WithMessage("Loại chập nhận không được để trống.").
            GreaterThan(0).WithMessage("Loại chập nhận phải lớn hơn 0").LessThan(4)
            .WithMessage("Loại chập nhận phải bé hơn 4.");

        RuleFor(x => x.DenyReason).NotEmpty().WithMessage("Lý do từ chối không được để trống");
    }
}
