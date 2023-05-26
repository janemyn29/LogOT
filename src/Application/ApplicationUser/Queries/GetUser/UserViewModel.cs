using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Application.ApplicationUser.Queries.GetUser;
public class UserViewModel
{
    public string Fullname { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public string IdentityNumber { get; set; }
    public DateTime BirthDay { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankAccountName { get; set; }
    public string BankName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

}
