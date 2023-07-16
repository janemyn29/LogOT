using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WebUI.Helper;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        throw new NotImplementedException();
    }
}
