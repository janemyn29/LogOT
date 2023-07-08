using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.DepartmentAllowance.Queries.GetDepartmentAllowance;

public class DepartmentAllowanceViewModel : IMapFrom<Domain.Entities.DepartmentAllowance>
{
    public Guid Id { get; set; }
    public string DepartmentId { get; set; }
    public string SubsidizeId { get; set; }

    public virtual Domain.Entities.Department Departments { get; set; }
    public virtual Domain.Entities.Subsidize Subsidize { get; set; }
}