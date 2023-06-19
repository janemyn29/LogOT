using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.SkillEmployee.Commands.CreateSkillEmployee;
public class CreateSkillEmployeeCommandViewModel : IMapFrom<Domain.Entities.SkillEmployee>
{
    public string ApplicationUserId { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
