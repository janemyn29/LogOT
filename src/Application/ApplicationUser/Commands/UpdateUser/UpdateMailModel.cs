using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Application.ApplicationUser.Commands.UpdateUser;
public class UpdateMailModel
{
    public string UserId { get; set; }
    public string NewEmail { get; set; }
}
