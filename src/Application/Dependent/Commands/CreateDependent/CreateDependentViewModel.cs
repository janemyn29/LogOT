using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.Dependent.Commands.CreateDependent;
public class CreateDependentViewModel : IMapFrom<Domain.Entities.Dependent>
{
    public string ApplicationUserId { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Desciption { get; set; }
    public string Relationship { get; set; }
}
