using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.ConfigWifis.Queries.GetList;
public class ConfigWifiViewModel : IMapFrom<Domain.Entities.ConfigWifi>
{
    public Guid Id { get; set; }
    public string NameWifi { get; set; }
    public string WifiIPv4 { get; set; }
}
