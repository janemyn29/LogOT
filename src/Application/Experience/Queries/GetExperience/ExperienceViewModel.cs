using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Experience.Queries.GetExperience;

public class ExperienceViewModel : IMapFrom<Domain.Entities.Experience>
{

    public Guid Id { get; init; }
    public string ApplicationUserId { get; set; }
    public string NameProject { get; set; }
    public int TeamSize { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string TechStack { get; set; }

    //public virtual ApplicationUser ApplicationUser { get; set; }

}