using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace EsriStatesApi
{
    public class HangfireAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
