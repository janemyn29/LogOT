using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Entities;

namespace mentor_v1.Application.Note.Queries.GetNotification;
public class NotificationViewModel : IMapFrom<Domain.Entities.Notification>
{
    //[ForeignKey("ApplicationUser")]
    public Guid Id { get; set; }
    public string ApplicationUserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRead { get; set; } = false;
    public virtual Domain.Identity.ApplicationUser ApplicationUser { get; set; }
}
