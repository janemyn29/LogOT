using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class Attendance: BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public DateTime Day { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ShiftEnum ShiftEnum { get; set; }
    public double? TimeWork { get; set; }
    public double? OverTime { get;set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

}
