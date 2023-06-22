using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.MaternityEmployee.Queries;
public class GetMaternityEmployeeViewModel : IMapFrom<Domain.Entities.MaternityEmployee>
{
    public Guid Id { get; set; }
    public string ApplicationUserId { get; set; }
    public string Image { get; set; }
    public string BirthDay { get; set; }
    public string AcceptanceType { get;set; }
    public string DenyReason { get;set; }

}
