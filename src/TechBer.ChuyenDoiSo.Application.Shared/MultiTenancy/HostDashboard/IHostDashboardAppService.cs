using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard
{
    public interface IHostDashboardAppService : IApplicationService
    {
        Task<TopStatsData> GetTopStatsData(GetTopStatsInput input);

        Task<GetRecentTenantsOutput> GetRecentTenantsData();

        Task<GetExpiringTenantsOutput> GetSubscriptionExpiringTenantsData();

        Task<GetIncomeStatisticsDataOutput> GetIncomeStatistics(GetIncomeStatisticsDataInput input);

        Task<GetEditionTenantStatisticsOutput> GetEditionTenantStatistics(GetEditionTenantStatisticsInput input);
        Task<BaoCaoLoaiDuAnOutput> GetBaoCaoDuAn();
    }
}