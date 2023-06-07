﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
public class CreateEmpContractValidator : AbstractValidator<EmpContractViewModel>
{
    public CreateEmpContractValidator(IApplicationDbContext context)
    {
        /*_context = context;

        // Add validation for request
        RuleFor(v => v.Fullname)
            .NotEmpty().WithMessage("Họ và tên không được để trống.")
            .MaximumLength(70).WithMessage("Họ và tên không được quá 70 ký tự.");
        // Add validation for request
        RuleFor(v => v.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống.")
            .MaximumLength(200).WithMessage("Địa chỉ không được quá 200 ký tự.");
        // Add validation for request
        RuleFor(v => v.IdentityNumber)
            .NotEmpty().WithMessage("Số cccd không được để trống.")
            .MaximumLength(12).WithMessage("Số cccd không được quá 12 ký tự.")
             .MustAsync(BeUniqueIdentity).WithMessage("Số cccd đã tồn tại!");
        // Add validation for request
        RuleFor(v => v.BirthDay)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.");

        RuleFor(v => v.BirthDay.AddYears(18)).LessThan(DateTime.Now).WithMessage("Ngày sinh chưa đủ 18 tuổi.");
        // Add validation for request
        RuleFor(v => v.BankName)
            .NotEmpty().WithMessage("Tên Ngân Hàng không được để trống.")
            .MaximumLength(100).WithMessage("Tên Ngân Hàng không được quá 100 ký tự.");
        // Add validation for request
        RuleFor(v => v.BankAccountNumber)
            .NotEmpty().WithMessage("Số tài khoản không được để trống.")
            .MaximumLength(70).WithMessage("Số tài khoản không được quá 70 ký tự.");
        // Add validation for request
        RuleFor(v => v.BankAccountName)
            .NotEmpty().WithMessage("Tên tài khoản ngân hàng không được để trống.")
            .MaximumLength(70).WithMessage("Tên tài khoản ngân hàng không được quá 70 ký tự.");*/
    }

    // Custom action to check with the database
    /*public async Task<bool> BeUniqueIdentity(string identity, CancellationToken cancellationToken)
    {
        var result = await _userManager.Users.Where(u => u.IdentityNumber == identity).FirstOrDefaultAsync();
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
