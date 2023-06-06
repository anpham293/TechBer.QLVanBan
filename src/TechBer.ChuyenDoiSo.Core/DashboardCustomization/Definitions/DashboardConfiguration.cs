using System.Collections.Generic;
using System.Linq;
using Abp.MultiTenancy;
using TechBer.ChuyenDoiSo.Authorization;

namespace TechBer.ChuyenDoiSo.DashboardCustomization.Definitions
{
    public class DashboardConfiguration
    {
        public List<DashboardDefinition> DashboardDefinitions { get; } = new List<DashboardDefinition>();

        public List<WidgetDefinition> WidgetDefinitions { get; } = new List<WidgetDefinition>();

        public List<WidgetFilterDefinition> WidgetFilterDefinitions { get; } = new List<WidgetFilterDefinition>();

        public DashboardConfiguration()
        {
            #region FilterDefinitions

            // These are global filter which all widgets can use
            var dateRangeFilter = new WidgetFilterDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                "FilterDateRangePicker"
            );

            WidgetFilterDefinitions.Add(dateRangeFilter);

            // Add your filters here

            #endregion

            #region WidgetDefinitions

            // Define Widgets

            #region TenantWidgets

            var tenantWidgetsDefaultPermission = new List<string>
            {
                AppPermissions.Pages_Tenant_Dashboard
            };

            var dailySales = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                "WidgetDailySales",
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string> { dateRangeFilter.Id },
                permissions: tenantWidgetsDefaultPermission
            );

            var generalStats = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                "WidgetGeneralStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission.Concat(new List<string>{ AppPermissions.Pages_Administration_AuditLogs }).ToList());

            var profitShare = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                "WidgetProfitShare",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var memberActivity = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                "WidgetMemberActivity",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var regionalStats = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                "WidgetRegionalStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var salesSummary = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                "WidgetSalesSummary",
                usedWidgetFilters: new List<string>() { dateRangeFilter.Id },
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            var topStats = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            // Báo cáo dashboard Admin tenant
            var tenantWidgetsAdminPermission = new List<string>
            {
                AppPermissions.Pages_Admin_Tenant_Widgets
            };

            var baoCaoChamDiem = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoChuyenDoiSo,
                "WidgetBaoCao",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsAdminPermission);

            var baoCaoTongHop = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoTongHop,
                "WidgetBaoCaoTongHop",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsAdminPermission);

            var baoCaoDoiTuong = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoDoiTuong,
                "WidgetBaoCaoDoiTuong",
                side: MultiTenancySides.Tenant,
                permissions: tenantWidgetsDefaultPermission);

            WidgetDefinitions.Add(generalStats);
            WidgetDefinitions.Add(dailySales);
            WidgetDefinitions.Add(profitShare);
            WidgetDefinitions.Add(memberActivity);
            WidgetDefinitions.Add(regionalStats);
            WidgetDefinitions.Add(topStats);
            WidgetDefinitions.Add(salesSummary);
            WidgetDefinitions.Add(baoCaoChamDiem);
            WidgetDefinitions.Add(baoCaoTongHop);
            WidgetDefinitions.Add(baoCaoDoiTuong);

            // Add your tenant side widgets here

            #endregion

            #region HostWidgets

            var hostWidgetsDefaultPermission = new List<string>
            {
                AppPermissions.Pages_Administration_Host_Dashboard
            };

            var incomeStatistics = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                "WidgetIncomeStatistics",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var hostTopStats = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var editionStatistics = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                "WidgetEditionStatistics",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var subscriptionExpiringTenants = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                "WidgetSubscriptionExpiringTenants",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission);

            var recentTenants = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                "WidgetRecentTenants",
                side: MultiTenancySides.Host,
                usedWidgetFilters: new List<string>() { dateRangeFilter.Id },
                permissions: hostWidgetsDefaultPermission);

            var baoCaoDuAn = new WidgetDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.BaoCaoDuAn,
                "WidgetBaoCaoDuAn",
                side: MultiTenancySides.Host,
                permissions: hostWidgetsDefaultPermission
            );
            
            WidgetDefinitions.Add(incomeStatistics);
            WidgetDefinitions.Add(hostTopStats);
            WidgetDefinitions.Add(editionStatistics);
            WidgetDefinitions.Add(subscriptionExpiringTenants);
            WidgetDefinitions.Add(recentTenants);
            WidgetDefinitions.Add(baoCaoDuAn);

            // Add your host side widgets here

            #endregion

            #endregion

            #region DashboardDefinitions

            // Create dashboard
            var defaultTenantDashboard = new DashboardDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard,
                new List<string>
                {
                    generalStats.Id, 
                    dailySales.Id, 
                    profitShare.Id, 
                    memberActivity.Id, 
                    regionalStats.Id, 
                    topStats.Id, 
                    salesSummary.Id, 
                    baoCaoChamDiem.Id,
                    baoCaoTongHop.Id,
                    baoCaoDoiTuong.Id
                });

            DashboardDefinitions.Add(defaultTenantDashboard);

            var defaultHostDashboard = new DashboardDefinition(
                ChuyenDoiSoDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard,
                new List<string>
                {
                    incomeStatistics.Id,
                    hostTopStats.Id,
                    editionStatistics.Id,
                    subscriptionExpiringTenants.Id,
                    recentTenants.Id,
                    baoCaoDuAn.Id
                });

            DashboardDefinitions.Add(defaultHostDashboard);

            // Add your dashboard definiton here

            #endregion

        }

    }
}
