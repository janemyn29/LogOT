using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Domain.Entities;
public class Allowance : BaseAuditableEntity
{
    public string Name { get; set; }
    public AllowanceType AllowanceType { get; set; }
    public double Amount { get; set; }
    public string Eligibility_Criteria { get; set; }
    public string Requirements { get; set; }

    public IList<AllowanceEmployee> AllowanceEmployees { get; private set; }
}
