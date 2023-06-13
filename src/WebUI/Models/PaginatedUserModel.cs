using mentor_v1.Application.Common.PagingUser;
using mentor_v1.Domain.Identity;

namespace WebUI.Models;

public class PaginatedUserModel<T> where T : class
{
    public T Defaut { get; set; }
    public PagingAppUser<ApplicationUser>  ListUser { get; set; }
}
