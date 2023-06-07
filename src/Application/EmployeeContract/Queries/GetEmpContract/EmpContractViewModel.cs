using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace mentor_v1.Application.EmployeeContract.Queries.GetEmpContract;
public class EmpContractViewModel
{
    public string ApplicationUserId { get; set; }
    public IFormFile? File { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Job { get; set; }
    public double? BasicSalary { get; set; }
    public EmployeeContractStatus Status { get; set; }
    public double? PercentDeduction { get; set; }
    public SalaryType SalaryType { get; set; }
    public ContractType ContractType { get; set; }
}