using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Level.Queries.GetLevel;

public class LevelViewModel : IMapFrom<Domain.Entities.Level>
{

    //public Guid Id { get; init; }
    public string Name { get; set; }

    public string Description { get; set; }

    //public IList<Position> Positions { get; set; }

}