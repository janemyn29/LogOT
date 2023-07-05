using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class Department : BaseAuditableEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public IList<Position> Positions { get; set; }
    public IList<DepartmentAllowance> DepartmentAllowances { get; private set; }

}