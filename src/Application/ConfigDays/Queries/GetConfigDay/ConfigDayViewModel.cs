using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.ConfigDays.Queries.GetConfigDay;
public class ConfigDayViewModel : IMapFrom<Domain.Entities.ConfigDay>
{
    public Guid Id { get; set; }
    public string Normal { get; set; }
    public string Saturday { get; set; }
    public string Sunday { get; set; }
    public string Holiday { get; set; }
}
