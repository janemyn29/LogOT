using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    
    public virtual Employee Employee { get; set; }
}
