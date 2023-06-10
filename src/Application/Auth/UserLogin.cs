using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Application.Auth;
public class UserLogin
{
    public string userId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }

    public string Token { get; set; }
    public List<string> listRoles { get; set; }
}
