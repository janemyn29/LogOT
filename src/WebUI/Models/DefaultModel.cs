using mentor_v1.Application.Common.Models;
using mentor_v1.Domain.Identity;

namespace WebUI.Models;

public class DefaultModel<T> where T : class
{
    public ApplicationUser User { get; set; }
    public T ListItem { get; set; }
}
