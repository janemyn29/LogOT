using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class ShiftConfig:BaseAuditableEntity
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set;}
    public ShiftEnum ShiftEnum { get; set;}
}
