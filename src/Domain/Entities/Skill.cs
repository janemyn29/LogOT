using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogOT.Domain.Entities;
public class Skill : BaseAuditableEntity
{

    public string SkillName { get; set; }
    public string Skill_Description { get; set; }


    // Relationship
    public IList<SkillEmployee> SkillEmployees { get; set; }
    public IList<SkillJD> SkillJDs { get; set; }
}
