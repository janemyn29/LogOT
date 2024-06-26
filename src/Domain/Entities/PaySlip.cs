﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class PaySlip : BaseAuditableEntity
{

    [ForeignKey("EmployeeContract")]
    public Guid EmployeeContractId { get; set; }
    public string Code { get; set; }
    public double SalaryPerHour { get; set; }
    public int? Standard_Work_Hours { get; set; }
    public int? Actual_Work_Hours { get; set; }
    public int? Ot_Hours { get; set; }
    public int? Leave_Hours { get; set; }
    public double? DefaultSalary { get; set; }
    public SalaryType SalaryType { get; set; }
    public InsuranceType InsuranceType { get; set; }
    public double InsuranceAmount { get; set; } //mức đóng BH ko lớn hơn mức 20 lần lương lương cơ sở và ko thấp hơn lương tối thiểu của vùng. 
    public bool isPersonalTaxDeduction { get; set; }
    public double PersonalTaxDeductionAmount { get; set; }
    public double DependentTaxDeductionAmount { get; set; }
    public RegionType RegionType { get; set; }
    public double RegionMinimumWage { get; set; }
    public bool IsMaternity { get; set; }=false;


    public double BHXH_Emp_Amount { get; set; }
    public double BHYT_Emp_Amount { get; set; }
    public double BHTN_Emp_Amount { get; set; }
    public double BHXH_Comp_Amount { get; set; }
    public double BHYT_Comp_Amount { get; set; }
    public double BHTN_Comp_Amount { get; set; }

    public double BHXH_Emp_Percent { get; set; }
    public double BHYT_Emp_Percent { get; set; }
    public double BHTN_Emp_Percent { get; set; }
    public double BHXH_Comp_Percent { get; set; }
    public double BHYT_Comp_Percent { get; set; }
    public double BHTN_Comp_Percent { get; set; }

    public double TotalInsuranceEmp { get; set; }
    public double TotalInsuranceComp { get; set; }


    //kết thúc BH
    public double? LeaveWageDeduction { get; set; }
    public double TaxableSalary { get; set; } //lương chịu thếu (có trừ nghỉ)
    public double TaxPercent { get; set; } // mức phần trăm chịu thuế
    // có mức giảm trừ gia cảnh ở trên 
    public int NumberOfDependent { get; set; }
    // có mức giảm trừ người phụ thuộc ở trên 
    public double TotalDependentAmount { get; set; }
    public double TotalTaxIncome { get; set; }
    public double AfterTaxSalary { get; set; }


    public double? TotalDepartmentAllowance  { get; set; }
    public double? TotalContractAllowance { get; set; }
    public double? OTWage { get; set; }

    public double? FinalSalary { get; set; }
    public DateTime? Paid_date { get; set; }
    public string? Note { get; set; }
    public string? BankName { get; set; }
    public string? BankAcountName { get; set; }
    public string? BankAcountNumber { get; set; }
    public virtual EmployeeContract EmployeeContract { get; set; } = null!;
}
