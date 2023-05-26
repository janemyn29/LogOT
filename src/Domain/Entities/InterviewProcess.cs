using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class InterviewProcess : BaseAuditableEntity
{

    [ForeignKey("Employee")]
    public string ApplicationUserId { get; set; }
    [ForeignKey("JobDescription")]
    public Guid JobDescriptionId { get; set; }
    public string Info { get; set; }
    public string DayTime { get; set; }
    public string Place { get; set; }
    public InterviewStatus Status { get; set; }
    public string FeedBack { get; set; }
    public string Result { get; set; }

    // Relationship
    public virtual JobDescription JobDescription { get; set; }


    public virtual ApplicationUser ApplicationUser { get; set; }
}
