using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class SkillEmployee : BaseAuditableEntity
{
    [ForeignKey("Employee")]
    public Guid EmployeeId { get; set; }
    [ForeignKey("Skill")]
    public Guid SkillId { get; set; }
    public LevelEnum Level { get; set; }
    //Relationship

    public virtual Employee Employee { get; set; }
    public virtual Skill Skill { get; set; }
}
