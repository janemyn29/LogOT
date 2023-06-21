using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using mentor_v1.Application.Common.Mappings;

namespace mentor_v1.Application.Degree.Commands.UpdateDegree;
public class UpdateDegreeViewModel : IMapFrom<Domain.Entities.Degree>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; }
    public int DegreeType { get; set; }
}
