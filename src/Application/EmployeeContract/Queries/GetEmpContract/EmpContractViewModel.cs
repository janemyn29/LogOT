using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
public class EmpContractViewModel : IMapFrom<Domain.Entities.EmployeeContract>
{
    public string Username { get; set; }
    public Guid Id { get; set; }
    public string ContractCode { get; set; }
    public string File { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Job { get; set; }
    public double? BasicSalary { get; set; }
    public string Status { get; set; }
    public double? PercentDeduction { get; set; }
    public string SalaryType { get; set; }
    public string ContractType { get; set; }

    public bool isPersonalTaxDeduction { get; set; }
    public string InsuranceType { get; set; }
    public double? InsuranceAmount { get; set; }
    public virtual Domain.Identity.ApplicationUser ApplicationUser { get; set; }
}