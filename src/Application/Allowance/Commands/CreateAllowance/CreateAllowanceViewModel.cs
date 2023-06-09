using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Application.Allowance.Commands.CreateAllowance;
public class CreateAllowanceViewModel
{
    public string Name { get; set; }
    public int AllowanceType { get; set; }
    public float Amount { get; set; }
    public string Eligibility_Criteria { get; set; }
    public string Requirements { get; set; }
}
