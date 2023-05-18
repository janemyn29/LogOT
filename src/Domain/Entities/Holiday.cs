using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogOT.Domain.Entities;
public class Holiday : BaseAuditableEntity
{
    [ForeignKey("Company")]
    public Guid CompanyId { get; set; }
    public string DateName { get; set; }
    public DateTime Day { get; set; }
    public double HourlyPay { get; set; }

    //Relationship
    public virtual Company Company { get; set; }
}
