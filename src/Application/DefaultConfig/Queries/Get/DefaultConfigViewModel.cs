using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.DefaultConfig.Queries.Get;
public class DefaultConfigViewModel : IMapFrom<Domain.Entities.DefaultConfig>
{
    public string CompanyRegionType { get; set; }
    public double BaseSalary { get; set; }
    public double PersonalTaxDeduction { get; set; }
    public double DependentTaxDeduction { get; set; }
    public int InsuranceLimit { get; set; }
}
