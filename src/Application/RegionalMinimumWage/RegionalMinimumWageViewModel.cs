using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.RegionalMinimumWage;
public class RegionalMinimumWageViewModel : IMapFrom<Domain.Entities.RegionalMinimumWage>
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public RegionType RegionType { get; set; }
}
