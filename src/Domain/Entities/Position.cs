using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class Position : BaseAuditableEntity
{
    [ForeignKey("Department")]
    public Guid DepartmentId { get; set; }

    [ForeignKey("Level")]
    public Guid LevelId { get; set; }
    public string Name { get; set; }

    public virtual Department Department { get; set; }
    public virtual Level Level { get; set; }

    public IList<ApplicationUser> ApplicationUsers { get; set; }
}

