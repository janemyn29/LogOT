using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class ExcelEmployeeQuit:BaseAuditableEntity
{

    [ForeignKey("JobReport")]
    public Guid JobReportId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Identity { get; set; }
    public WorkStatus WorkStatus { get; set; }
    public ActionType ActionType { get; set; }
    public DateTime ActionDate { get; set; }
    public virtual JobReport JobReport { get; set; }
}
