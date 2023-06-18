using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class RegionalMinimumWage :  BaseAuditableEntity
{
    public double Amount { get; set; }
    public RegionType  RegionType { get; set; }
}
