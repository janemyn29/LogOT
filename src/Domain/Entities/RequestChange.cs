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

    public string? Fullname { get; set; }
    public string? Address { get; set; }
    public string? Image { get; set; }

    public string? Phone { get; set; }
    public string? IdentityNumber { get; set; }
    public DateTime? BirthDay { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankName { get; set; }

    public string? Email { get; set; }

    public string? DenyReason { get; set; }
    public RequestStatus Status { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
}
