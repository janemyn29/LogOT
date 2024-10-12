using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Payslip.Queries;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Payslip.Commands.Create;
public class CreatePayslipCommand : IRequest<Guid>
{
   public PayslipViewModel payslip { get; set; }
}

public class CreatePayslipCommandHandler : IRequestHandler<CreatePayslipCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CreatePayslipCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager, IMapper mapper )
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePayslipCommand request, CancellationToken cancellationToken)
    {

        var item = new Domain.Entities.PaySlip()
        {
            EmployeeContractId = request.payslip.EmployeeContractId,
        Code = request.payslip.Code,
        FromTime = request.payslip.FromTime,
        ToTime = request.payslip.ToTime,
        PaydayCal = request.payslip.PaydayCal,
        SalaryPerHour = request.payslip.SalaryPerHour,
        Standard_Work_Hours = request.payslip.Standard_Work_Hours,
        Actual_Work_Hours = request.payslip.Actual_Work_Hours,
        Ot_Hours = request.payslip.Ot_Hours,
        Leave_Hours = request.payslip.Leave_Hours,
        DefaultSalary = request.payslip.DefaultSalary,
       SalaryType = request.payslip.SalaryType,
        InsuranceType = request.payslip.InsuranceType,
        InsuranceAmount = request.payslip.InsuranceAmount,
        isPersonalTaxDeduction = request.payslip.isPersonalTaxDeduction,
        PersonalTaxDeductionAmount = request.payslip.PersonalTaxDeductionAmount,
        DependentTaxDeductionAmount = request.payslip.DependentTaxDeductionAmount,
        RegionType = request.payslip.RegionType,
        RegionMinimumWage = request.payslip.RegionMinimumWage,
        IsMaternity = request.payslip.IsMaternity,
        MaternityHour = request.payslip.MaternityHour,

        BHXH_Emp_Amount = request.payslip.BHXH_Emp_Amount,
        BHYT_Emp_Amount = request.payslip.BHYT_Emp_Amount,
        BHTN_Emp_Amount = request.payslip.BHTN_Emp_Amount,
        BHXH_Emp_Percent = request.payslip.BHXH_Emp_Percent,
        BHYT_Emp_Percent = request.payslip.BHYT_Emp_Percent,
        BHTN_Emp_Percent = request.payslip.BHTN_Emp_Percent,

        BHXH_Comp_Amount = request.payslip.BHXH_Comp_Amount,
        BHYT_Comp_Amount = request.payslip.BHYT_Comp_Amount,
       BHTN_Comp_Amount = request.payslip.BHTN_Comp_Amount,
        BHXH_Comp_Percent = request.payslip.BHXH_Comp_Percent,
        BHYT_Comp_Percent = request.payslip.BHYT_Comp_Percent,
        BHTN_Comp_Percent = request.payslip.BHTN_Comp_Percent,

        TotalInsuranceEmp = request.payslip.TotalInsuranceEmp,
        TotalInsuranceComp = request.payslip.TotalInsuranceComp,
        LeaveWageDeduction = request.payslip.LeaveWageDeduction,
        TaxableSalary = request.payslip.TaxableSalary,
        NumberOfDependent = request.payslip.NumberOfDependent,
       TotalDependentAmount = request.payslip.TotalDependentAmount,
        TNTT = (double)request.payslip.TNTT,
        TotalTaxIncome = request.payslip.TotalTaxIncome,
        AfterTaxSalary = request.payslip.AfterTaxSalary,
       TotalDepartmentAllowance = request.payslip.TotalDepartmentAllowance,
        TotalContractAllowance = request.payslip.TotalContractAllowance,

        OTWage = request.payslip.OTWage,
        FinalSalary = request.payslip.FinalSalary,
        Note = request.payslip.Note,

       BankName = request.payslip.BankName,
        BankAcountName = request.payslip.BankAcountName,
        BankAcountNumber = request.payslip.BankAcountNumber
    };
        // add new category
        _context.Get<Domain.Entities.PaySlip>().Add(item);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
}

