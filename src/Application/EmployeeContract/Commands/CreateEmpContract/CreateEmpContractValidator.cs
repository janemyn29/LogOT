using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.DefaultConfig.Queries.Get;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.EmployeeContract.Commands.CreateEmpContract;
public class CreateEmpContractValidator : AbstractValidator<CreateEmployeeContractCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateEmpContractValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.ContractCode)
            .NotEmpty().WithMessage("Mã hợp đồng không thể để trống.")
            .MaximumLength(70).WithMessage("Mã hợp đồng không được quá 70 ký tự.")
             .MustAsync(BeUniqueCode).WithMessage("Mã hợp đồng này đã tồn tại!");
        // Add validation for request
        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("Ngày bắt đầu hợp đồng không được để trống.").GreaterThan(DateTime.Now).WithMessage("Ngày bắt đầu hợp đồng không thể trùng hoặc nhỏ hơn ngày hiện tại!");
        // Add validation for request
        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Tên người dùng không được để trống.");
        // Add validation for request
        RuleFor(v => v.File)
            .NotEmpty().WithMessage("File hợp đồng không thể để trống!");
        // Add validation for request
        RuleFor(v => v.Job)
            .NotEmpty().WithMessage("Công việc không thể để trống.")
            .MaximumLength(200).WithMessage("Tên Ngân Hàng không được quá 100 ký tự.");
        // Add validation for request
        RuleFor(v => v.BasicSalary)
            .NotEmpty().WithMessage("Lương cơ bản không được để trống.").GreaterThan(0).WithMessage("Lương cơ bản không được nhỏ hơn hoặc bằng 0.");
        RuleFor(v => v.ContractType)
            .NotEmpty().WithMessage("Loại hợp đồng không được để trống.");
        RuleFor(v => v.SalaryType)
            .NotEmpty().WithMessage("Loại lương không được để trống.");
        RuleFor(v => v.isPersonalTaxDeduction)
           .NotEmpty().WithMessage("Yêu cầu tính giảm trừ gia cảnh bản thân không được để trống.");
        RuleFor(v => v.InsuranceType)
           .NotEmpty().WithMessage("Hình thức nộp bảo hiểm không được để trống.");
    }

    // Custom action to check with the database
    public async Task<bool> BeUniqueCode(string identity, CancellationToken cancellationToken)
    {
        var result = await _context.Get<Domain.Entities.EmployeeContract>().Where(u => u.ContractCode == identity).FirstOrDefaultAsync();
        if (result == null)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }
}
