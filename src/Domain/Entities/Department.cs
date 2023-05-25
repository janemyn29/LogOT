using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class Department : BaseAuditableEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public IList<Employee> Employees { get; set; }

    public IList<Position> Positions { get; set; }

    // Tổng nhân viên hiện có
    public int GetTotalEmployees()
    {
        if (Employees == null)
        {
            return 0;
        }

        return Employees.Count();
    }
}
