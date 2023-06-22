using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.Allowance.Commands.UpdateAllowance;
public class UpdateAllowanceViewModel : IMapFrom<Domain.Entities.Allowance>
{ 
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int AllowanceType { get; set; }
    public float Amount { get; set; }
    public string Eligibility_Criteria { get; set; }
    public string Requirements { get; set; }
}
