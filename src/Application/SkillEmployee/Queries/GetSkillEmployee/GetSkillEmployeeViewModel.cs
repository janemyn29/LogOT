using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.SkillEmployee.Queries.GetSkillEmployee;
public class GetSkillEmployeeViewModel : IMapFrom<Domain.Entities.SkillEmployee>
{
    public Guid Id { get; set; }
    public string ApplicationUserId { get; set; }
    public string Level { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
