using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class DefaultConfig : BaseAuditableEntity
{
    public RegionType CompanyRegionType { get; set; }
    public double BaseSalary { get; set; }
    public double PersonalTaxDeduction { get; set;}
    public double DependentTaxDeduction { get; set; }
    public int InsuranceLimit { get; set; }
}
