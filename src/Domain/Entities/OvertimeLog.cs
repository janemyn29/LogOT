using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogOT.Domain.Entities;
public class OvertimeLog : BaseAuditableEntity
{
    [ForeignKey("Employee")]
    public Guid EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
    public string? CancelReason { get; set; }
    public LogStatus Status { get; set; }
    public virtual Employee Employee { get; set; }
}