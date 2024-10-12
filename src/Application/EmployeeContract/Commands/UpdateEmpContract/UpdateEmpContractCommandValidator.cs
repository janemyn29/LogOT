using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;

namespace mentor_v1.Application.EmployeeContract.Commands.UpdateEmpContract;
public class UpdateEmpContractCommandValidator : AbstractValidator<UpdateEmpContractCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateEmpContractCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Add validation for request
        RuleFor(v => v.ContractCode)
            .NotEmpty().WithMessage("Mã hợp đồng không thể để trống.")
            .MaximumLength(70).WithMessage("Mã hợp đồng không được quá 70 ký tự.");
        // Add validation for request
        RuleFor(v => v.StartDate)
           .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.").GreaterThan(DateTime.Now).WithMessage("Ngày bắt đầu không thể trùng hoặc nhỏ hơn ngày hiện tại!");

        // Add validation for request
        RuleFor(v => v.File)
            .NotEmpty().WithMessage("File hợp đồng không thể để trống!");
        // Add validation for request
        RuleFor(v => v.Job)
            .NotEmpty().WithMessage("Công việc không thể để trống.")
            .MaximumLength(200).WithMessage("Tên Ngân Hàng không được quá 100 ký tự.");
        // Add validation for request
        RuleFor(v => v.BasicSalary)
            .NotEmpty().WithMessage("Lương cơ bản không được để trống.");
        // Add validation for request
        RuleFor(v => v.Status)
            .NotEmpty().WithMessage("Trạng thái hợp đồng không được để trống.");
        RuleFor(v => v.ContractType)
            .NotEmpty().WithMessage("Loại hợp đồng không được để trống.");
        RuleFor(v => v.SalaryType)
            .NotEmpty().WithMessage("Loại lương không được để trống.");
        RuleFor(v => v.isPersonalTaxDeduction)
           .NotEmpty().WithMessage("Yêu cầu tính giảm trừ gia cảnh bản thân không được để trống.");
        RuleFor(v => v.InsuranceType)
           .NotEmpty().WithMessage("Hình thức nộp bảo hiểm không được để trống.");
    }
}

