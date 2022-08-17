using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechBer.ChuyenDoiSo.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.HealthChecks
{
    public class ChuyenDoiSoDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public ChuyenDoiSoDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("ChuyenDoiSoDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("ChuyenDoiSoDbContext could not connect to database"));
        }
    }
}
