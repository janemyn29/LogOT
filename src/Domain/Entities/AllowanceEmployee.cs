using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class AllowanceEmployee :BaseAuditableEntity
{
    [ForeignKey("EmployeeContract")]
    public Guid EmployeeContractId { get; set; }
    [ForeignKey("Allowance")]
    public Guid AllowanceId { get; set; }

    public virtual EmployeeContract EmployeeContract { get; set; }
    public virtual Allowance Allowance { get; set; }

}
