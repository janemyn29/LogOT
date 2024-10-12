
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
public class OvertimeLogViewModel : IMapFrom<Domain.Entities.OvertimeLog>
{
    public Guid Id { get; set; }    
    public DateTime Date { get; set; }
    public int Hours { get; set; }
    public string? CancelReason { get; set; }
    public string Status { get; set; }
}
