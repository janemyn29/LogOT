using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentor_v1.Application.ApplicationUser.Queries.GetUser;
public class Dashboard
{
    public string Title { get; set; }
    public string Detail { get; set; }
    public Dashboard(string title, string detail)
    {
        Title = title;
        Detail = detail;
    }
}
