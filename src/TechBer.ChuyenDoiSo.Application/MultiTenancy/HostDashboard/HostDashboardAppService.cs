using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Timing;
using Microsoft.EntityFrameworkCore;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class HostDashboardAppService : ChuyenDoiSoAppServiceBase, IHostDashboardAppService
    {
        private const int SubscriptionEndAlertDayCount = 30;
        private const int MaxExpiringTenantsShownCount = 10;
        private const int MaxRecentTenantsShownCount = 10;
        private const int RecentTenantsDayCount = 7;

        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IIncomeStatisticsService _incomeStatisticsService;
        private readonly IRepository<DuAn> _duAnRepository;
        private readonly IRepository<VanBanDuAn> _vanBanDuAnRepository;
        private readonly IRepository<LoaiDuAn> _loaiDuAnRepository;

        public HostDashboardAppService(
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IRepository<Tenant> tenantRepository,
            IIncomeStatisticsService incomeStatisticsService,
            IRepository<DuAn> duAnRepository,
            IRepository<VanBanDuAn> vanBanDuAnRepository,
            IRepository<LoaiDuAn> loaiDuAnRepository
            )
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _tenantRepository = tenantRepository;
            _incomeStatisticsService = incomeStatisticsService;
            _duAnRepository = duAnRepository;
            _vanBanDuAnRepository = vanBanDuAnRepository;
            _loaiDuAnRepository = loaiDuAnRepository;
        }

        public async Task<TopStatsData> GetTopStatsData(GetTopStatsInput input)
        {
            return new TopStatsData
            {
                DashboardPlaceholder1 = 125,
                DashboardPlaceholder2 = 830,
                NewTenantsCount = await GetTenantsCountByDate(input.StartDate, input.EndDate),
                NewSubscriptionAmount = GetNewSubscriptionAmount(input.StartDate, input.EndDate)
            };
        }

        public async Task<GetRecentTenantsOutput> GetRecentTenantsData()
        {
            var tenantCreationStartDate = Clock.Now.ToUniversalTime().AddDays(-RecentTenantsDayCount);

            var recentTenants = await GetRecentTenantsData(tenantCreationStartDate, MaxRecentTenantsShownCount);

            return new GetRecentTenantsOutput()
            {
                RecentTenants = recentTenants,
                TenantCreationStartDate = tenantCreationStartDate,
                RecentTenantsDayCount = RecentTenantsDayCount,
                MaxRecentTenantsShownCount = MaxRecentTenantsShownCount
            };
        }

        public async Task<GetExpiringTenantsOutput> GetSubscriptionExpiringTenantsData()
        {
            var subscriptionEndDateEndUtc = Clock.Now.ToUniversalTime().AddDays(SubscriptionEndAlertDayCount);
            var subscriptionEndDateStartUtc = Clock.Now.ToUniversalTime();

            var expiringTenants = await GetExpiringTenantsData(subscriptionEndDateStartUtc, subscriptionEndDateEndUtc,
                MaxExpiringTenantsShownCount);

            return new GetExpiringTenantsOutput()
            {
                ExpiringTenants = expiringTenants,
                MaxExpiringTenantsShownCount = MaxExpiringTenantsShownCount,
                SubscriptionEndAlertDayCount = SubscriptionEndAlertDayCount,
                SubscriptionEndDateStart = subscriptionEndDateStartUtc,
                SubscriptionEndDateEnd = subscriptionEndDateEndUtc
            };
        }

        public async Task<GetIncomeStatisticsDataOutput> GetIncomeStatistics(GetIncomeStatisticsDataInput input)
        {
            return new GetIncomeStatisticsDataOutput(
                await _incomeStatisticsService.GetIncomeStatisticsData(
                    input.StartDate, 
                    input.EndDate,
                    input.IncomeStatisticsDateInterval)
            );
        }

        public async Task<GetEditionTenantStatisticsOutput> GetEditionTenantStatistics(GetEditionTenantStatisticsInput input)
        {
            return new GetEditionTenantStatisticsOutput(
                await GetEditionTenantStatisticsData(input.StartDate, input.EndDate)
            );
        }

        private async Task<List<TenantEdition>> GetEditionTenantStatisticsData(DateTime startDate, DateTime endDate)
        {
            return (await _tenantRepository.GetAll()
                .Where(t => t.EditionId.HasValue &&
                            t.IsActive &&
                            t.CreationTime >= startDate &&
                            t.CreationTime <= endDate)
                .Select(t => new { t.EditionId, t.Edition.DisplayName })
                .ToListAsync()
                )
                .GroupBy(t => t.EditionId)
                .Select(t => new TenantEdition
                {
                    Label = t.First().DisplayName,
                    Value = t.Count()
                })
                .OrderBy(t => t.Label)
                .ToList();
        }

        private decimal GetNewSubscriptionAmount(DateTime startDate, DateTime endDate)
        {
            return  _subscriptionPaymentRepository.GetAll()
                  .Where(s => s.CreationTime >= startDate &&
                              s.CreationTime <= endDate &&
                              s.Status == SubscriptionPaymentStatus.Paid)
                  .Select(x => x.Amount).AsEnumerable()
                  .Sum();
        }

        private async Task<int> GetTenantsCountByDate(DateTime startDate, DateTime endDate)
        {
            return await _tenantRepository.GetAll()
                .Where(t => t.CreationTime >= startDate && t.CreationTime <= endDate)
                .CountAsync();
        }

        private async Task<List<ExpiringTenant>> GetExpiringTenantsData(DateTime subscriptionEndDateStartUtc, DateTime subscriptionEndDateEndUtc, int? maxExpiringTenantsShownCount = null)
        {
            var query = _tenantRepository.GetAll()
                .Where(t =>
                    t.SubscriptionEndDateUtc.HasValue &&
                    t.SubscriptionEndDateUtc.Value >= subscriptionEndDateStartUtc &&
                    t.SubscriptionEndDateUtc.Value <= subscriptionEndDateEndUtc)
                .Select(t => new
                {
                    t.Name,
                    t.SubscriptionEndDateUtc
                });

            if (maxExpiringTenantsShownCount.HasValue)
            {
                query = query.Take(maxExpiringTenantsShownCount.Value);
            }

            return (await query.ToListAsync())
                .Select(t => new ExpiringTenant
                {
                    TenantName = t.Name,
                    RemainingDayCount = Convert.ToInt32(t.SubscriptionEndDateUtc.Value.Subtract(subscriptionEndDateStartUtc).TotalDays)
                })
                .OrderBy(t => t.RemainingDayCount)
                .ThenBy(t => t.TenantName)
                .ToList();
        }

        private async Task<List<RecentTenant>> GetRecentTenantsData(DateTime creationDateStart, int? maxRecentTenantsShownCount = null)
        {
            var query = _tenantRepository.GetAll()
                .Where(t => t.CreationTime >= creationDateStart)
                .OrderByDescending(t => t.CreationTime);

            if (maxRecentTenantsShownCount.HasValue)
            {
                query = (IOrderedQueryable<Tenant>)query.Take(maxRecentTenantsShownCount.Value);
            }

            return (await query.ToListAsync())
                .Select(t => ObjectMapper.Map<RecentTenant>(t))
                .ToList();
        }

        public async Task<BaoCaoLoaiDuAnOutput> GetBaoCaoDuAn()
        {
            var listBaoCaoLoaiDuAns = new List<ChiTietBaoCaoLoaiDuAn>();
            var loaiDuAns = _loaiDuAnRepository.GetAll().ToList();
            foreach (var loaiDuAn in loaiDuAns)
            {
                var chiTiet = new ChiTietBaoCaoLoaiDuAn();
                chiTiet.TenLoaiDuAn = loaiDuAn.Name;
                chiTiet.LoaiDuAn_SoDuAn =
                    _duAnRepository.GetAll().WhereIf(true, p => p.LoaiDuAnId == loaiDuAn.Id).Count();
                listBaoCaoLoaiDuAns.Add(chiTiet);
            }
            return new BaoCaoLoaiDuAnOutput()
            {
                listBaoCaoLoaiDuAns = listBaoCaoLoaiDuAns
            };
        }
    }
}