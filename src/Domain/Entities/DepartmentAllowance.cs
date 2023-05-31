using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class DepartmentAllowance : BaseAuditableEntity
{


    [ForeignKey("Department")]
    public Guid DepartmentId { get; set; }
    [ForeignKey("Subsidize")]
    public Guid SubsidizeId { get; set; }

    public virtual Department Departments { get; set; }
    public virtual Subsidize Subsidize { get; set; }


}

