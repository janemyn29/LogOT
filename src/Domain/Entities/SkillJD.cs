using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class SkillJD : BaseAuditableEntity
{
    [ForeignKey("Skill")]
    public Guid SkillId { get; set; }
    [ForeignKey("JobDescription")]
    public Guid JobDescriptionId { get; set; }
    public LevelEnum Level { get; set; }
    // Relationship
    public virtual Skill Skill { get; set; }
    public virtual JobDescription JobDescription { get; set; }
}
