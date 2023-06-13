using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Degree.Commands.CreateDegree;
public class CreateDegreeViewModel : IMapFrom<Domain.Entities.Degree>
{
    public Guid ApplicationUserId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; }
    public int DegreeType { get; set; }
}