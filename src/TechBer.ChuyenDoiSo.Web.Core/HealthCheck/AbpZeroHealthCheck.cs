using Microsoft.Extensions.DependencyInjection;
using TechBer.ChuyenDoiSo.HealthChecks;

namespace TechBer.ChuyenDoiSo.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<ChuyenDoiSoDbContextHealthCheck>("Database Connection");
            builder.AddCheck<ChuyenDoiSoDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
