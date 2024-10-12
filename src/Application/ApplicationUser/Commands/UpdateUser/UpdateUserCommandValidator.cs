using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public class UpdateUserCommandValidator: AbstractValidator<UpdateUserModel>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public UpdateUserCommandValidator(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
            .MaximumLength(12).WithMessage("Số cccd không được quá 12 ký tự.");
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
            .MaximumLength(70).WithMessage("Tên tài khoản ngân hàng không được quá 70 ký tự.");
        RuleFor(v => v.PhoneNumber)
           .NotEmpty().WithMessage("Số điện thoại không được để trống.")
           .MaximumLength(10).WithMessage("Số điện thoại không được quá 10 ký tự.");
    }
}
