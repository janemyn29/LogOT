using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Subsidize.Queries.GetSubsidize;

public class SubsidizeViewModel : IMapFrom<Domain.Entities.Subsidize>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }  

}