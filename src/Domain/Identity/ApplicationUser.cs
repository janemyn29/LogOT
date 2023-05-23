using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public string Fullname { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public virtual Employee Employee { get; set; }
}
