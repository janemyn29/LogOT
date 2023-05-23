using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Domain.Identity;

namespace mentor_v1.Domain.Entities;
public class RequestChange : BaseAuditableEntity
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }

    public string RequestName { get; set; }
    public string RequestDescription { get; set; }
    public string? DenyReason { get; set; }
    public RequestStatus Status { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
}
