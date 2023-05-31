using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class Dependent : BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Desciption { get; set; }

    public string Relationship { get; set; }
    public AcceptanceType AcceptanceType { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}

