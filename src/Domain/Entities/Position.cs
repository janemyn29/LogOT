using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class Position : BaseAuditableEntity
{
    [ForeignKey("Department")]
    public Guid DepartmentId { get; set; }

    [ForeignKey("Employee")]
    public Guid EmployeeId { get; set; }
    public string Name { get; set; }

    public PositionLevel PositionLevel { get; set; }

    public virtual Department Department { get; set; }
    public virtual Employee Employee { get; set; }
}
