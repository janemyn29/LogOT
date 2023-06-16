using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class Coefficient : BaseAuditableEntity
{
    public double AmountCoefficient { get; set; }
    public TypeDate TypeDate { get; set; } 
    public IList<AnnualWorkingDay> AnnualWorkingDays { get; set; }  
}
