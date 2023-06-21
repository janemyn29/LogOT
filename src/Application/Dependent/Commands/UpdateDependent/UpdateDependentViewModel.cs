using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.Dependent.Commands.UpdateDependent;
public class UpdateDependentViewModel : IMapFrom<Domain.Entities.Dependent>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Desciption { get; set; }
    public string Relationship { get; set; }
    public int AcceptanceType { get; set; }
}