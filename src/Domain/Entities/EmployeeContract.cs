using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class EmployeeContract : BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public string ContractCode { get; set; }
    public string? File { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Job { get; set; }
    public double? BasicSalary { get; set; }
    public EmployeeContractStatus Status { get; set; }
    public double? PercentDeduction { get; set; }
    public SalaryType SalaryType { get; set; }
    public ContractType ContractType { get; set; }
    public bool isPersonalTaxDeduction { get; set; }
    public InsuranceType InsuranceType { get; set; }
    public double? InsuranceAmount { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

    public IList<PaySlip> PaySlips { get; private set; }

    public IList<AllowanceEmployee> AllowanceEmployees { get; private set; }

}

