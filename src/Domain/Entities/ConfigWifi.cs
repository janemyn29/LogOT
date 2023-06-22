using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class ConfigWifi : BaseAuditableEntity
{
    public string NameWifi { get; set; }
    public string WifiIpv4 { get; set; }
}
