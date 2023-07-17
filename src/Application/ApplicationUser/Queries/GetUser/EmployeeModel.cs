using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.ApplicationUser.Queries.GetUser;
public class EmployeeModel
{
        public Guid PositionId { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public GenderType GenderType { get; set; } = GenderType.Other;
        public string IdentityNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsMaternity { get; set; } = false;
        public string Image { get; set; }
        public string PhoneNumber { get; set; }
         public string Role { get; set; } = "Employee"; 
        public string ContractCode { get; set; }
        public string? File { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Job { get; set; }
        public double? BasicSalary { get; set; }
        public double? PercentDeduction { get; set; }
        public SalaryType SalaryType { get; set; }
        public ContractType ContractType { get; set; }
        public bool isPersonalTaxDeduction { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public double? InsuranceAmount { get; set; }
        public Guid[]? AllowanceId { get; set; }
    }

