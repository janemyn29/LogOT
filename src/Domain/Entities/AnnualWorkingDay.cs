using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class AnnualWorkingDay : BaseAuditableEntity
{
    [ForeignKey("Coefficient")]
    public Guid CoefficientId { get; set; }
    public DateTime Day { get; set; }
    public ShiftType ShiftType { get; set; }
    public TypeDate TypeDate { get; set; }
    public virtual Coefficient Coefficient { get; set; }
}

