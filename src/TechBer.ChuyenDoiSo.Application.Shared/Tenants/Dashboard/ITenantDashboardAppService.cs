using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto;

namespace TechBer.ChuyenDoiSo.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();

        GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input);

        GetDailySalesOutput GetDailySales();

        GetProfitShareOutput GetProfitShare();

        GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input);

        GetTopStatsOutput GetTopStats();

        GetRegionalStatsOutput GetRegionalStats();

        GetGeneralStatsOutput GetGeneralStats();

        Task<List<BaoCaoChamDiemOutput>> GetBaoCaoChamDiem();

        Task<BaoCaoTongHopOutput> GetBaoCaoTongHop();

        Task<List<DiemCuaTieuChiOutput>> GetBaoCaoChamDiemDoiTuong(int idDoiTuong);
    }
}
