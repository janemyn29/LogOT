using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class ExcelContract: BaseAuditableEntity
{

    [ForeignKey("JobReport")]
    public Guid JobReportId { get; set; }
    public string ContractCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployeeName { get; set; }
    public string IdentityNumber { get; set; }
    public string ContractStatus { get; set; }
    public ActionType Action { get; set; }
    public DateTime ActionDate { get; set; }

    public virtual JobReport JobReport { get; set; }
}
