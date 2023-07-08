using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.ShiftConfig.Queries;
public class ShiftViewModel:IMapFrom<Domain.Entities.ShiftConfig>
{

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string ShiftEnum { get; set; }
}
