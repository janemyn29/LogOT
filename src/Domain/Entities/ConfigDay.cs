using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class ConfigDay:BaseAuditableEntity
{
    public ShiftType Normal { get; set; }
    public ShiftType Saturday { get; set; }
    public ShiftType Sunday { get; set; }
    public ShiftType Holiday { get; set; }
}
