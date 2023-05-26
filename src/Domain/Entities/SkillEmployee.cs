using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class SkillEmployee : BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    [ForeignKey("Skill")]
    public Guid SkillId { get; set; }
    public LevelEnum Level { get; set; }
    //Relationship

    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual Skill Skill { get; set; }
}
