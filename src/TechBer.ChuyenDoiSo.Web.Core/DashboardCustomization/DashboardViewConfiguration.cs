using System.Collections.Generic;


namespace TechBer.ChuyenDoiSo.Web.DashboardCustomization
{
    public class DashboardViewConfiguration
    {
        public Dictionary<string, WidgetViewDefinition> WidgetViewDefinitions { get; } = new Dictionary<string, WidgetViewDefinition>();

        public Dictionary<string, WidgetFilterViewDefinition> WidgetFilterViewDefinitions { get; } = new Dictionary<string, WidgetFilterViewDefinition>();

        public DashboardViewConfiguration()
        {
            var jsAndCssFileRoot = "/Areas/App/Views/CustomizableDashboard/Widgets/";
            var viewFileRoot = "~/Areas/App/Views/Shared/Components/CustomizableDashboard/Widgets/";

            #region FilterViewDefinitions

            WidgetFilterViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                new WidgetFilterViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                    viewFileRoot + "DateRangeFilter.cshtml",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.js",
                    jsAndCssFileRoot + "DateRangeFilter/DateRangeFilter.min.css")
            );
            
            //add your filters iew definitions here
            #endregion

            #region WidgetViewDefinitions

            #region TenantWidgets

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                    viewFileRoot + "DailySales.cshtml",
                    jsAndCssFileRoot + "DailySales/DailySales.min.js",
                    jsAndCssFileRoot + "DailySales/DailySales.min.css"));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                    viewFileRoot + "GeneralStats.cshtml",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.js",
                    jsAndCssFileRoot + "GeneralStats/GeneralStats.min.css"));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                    viewFileRoot + "ProfitShare.cshtml",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.js",
                    jsAndCssFileRoot + "ProfitShare/ProfitShare.min.css"));
  
            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                    viewFileRoot + "MemberActivity.cshtml",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.js",
                    jsAndCssFileRoot + "MemberActivity/MemberActivity.min.css"));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                    viewFileRoot + "RegionalStats.cshtml",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.js",
                    jsAndCssFileRoot + "RegionalStats/RegionalStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                    viewFileRoot + "SalesSummary.cshtml",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.js",
                    jsAndCssFileRoot + "SalesSummary/SalesSummary.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                    viewFileRoot + "TopStats.cshtml",
                    jsAndCssFileRoot + "TopStats/TopStats.min.js",
                    jsAndCssFileRoot + "TopStats/TopStats.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoChuyenDoiSo,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoChuyenDoiSo,
                    viewFileRoot + "BaoCaoChamDiem.cshtml",
                    jsAndCssFileRoot + "BaoCaoChamDiem/BaoCaoChamDiem.min.js",
                    jsAndCssFileRoot + "BaoCaoChamDiem/BaoCaoChamDiem.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoTongHop,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoTongHop,
                    viewFileRoot + "BaoCaoTongHop.cshtml",
                    jsAndCssFileRoot + "BaoCaoTongHop/BaoCaoTongHop.min.js",
                    jsAndCssFileRoot + "BaoCaoTongHop/BaoCaoTongHop.min.css",
                    12,
                    10));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoDoiTuong,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Tenant.BaoCaoDoiTuong,
                    viewFileRoot + "BaoCaoDoiTuong.cshtml",
                    jsAndCssFileRoot + "BaoCaoDoiTuong/BaoCaoDoiTuong.min.js",
                    jsAndCssFileRoot + "BaoCaoDoiTuong/BaoCaoDoiTuong.min.css",
                    12,
                    10));

            //add your tenant side widget definitions here
            #endregion

            #region HostWidgets

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                    viewFileRoot + "IncomeStatistics.cshtml",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.js",
                    jsAndCssFileRoot + "IncomeStatistics/IncomeStatistics.min.css"));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.TopStats,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.TopStats,
                    viewFileRoot + "HostTopStats.cshtml",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.js",
                    jsAndCssFileRoot + "HostTopStats/HostTopStats.min.css"));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                    viewFileRoot + "EditionStatistics.cshtml",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.js",
                    jsAndCssFileRoot + "EditionStatistics/EditionStatistics.min.css"));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                    viewFileRoot + "SubscriptionExpiringTenants.cshtml",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.js",
                    jsAndCssFileRoot + "SubscriptionExpiringTenants/SubscriptionExpiringTenants.min.css",
                    6,
                    10));

            WidgetViewDefinitions.Add(ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                new WidgetViewDefinition(
                    ChuyenDoiSoDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                    viewFileRoot + "RecentTenants.cshtml",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.js",
                    jsAndCssFileRoot + "RecentTenants/RecentTenants.min.css"));

            //add your host side widgets definitions here
            #endregion

            #endregion
        }
    }
}
