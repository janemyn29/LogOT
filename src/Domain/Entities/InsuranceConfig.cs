using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class InsuranceConfig: BaseAuditableEntity
{
    public double BHXH_Emp { get; set; }
    public double BHYT_Emp { get; set; }
    public double BHTN_Emp { get; set; }

    public double BHXH_Comp { get; set; }
    public double BHYT_Comp { get; set; }
    public double BHTN_Comp { get; set; }
    public double BHCD_Comp { get; set; }

}
