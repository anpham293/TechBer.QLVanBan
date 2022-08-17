using TechBer.ChuyenDoiSo.DashboardCustomization;
using TechBer.ChuyenDoiSo.DashboardCustomization.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.CustomizableDashboard
{
    public class CustomizableDashboardViewModel
    {
        public DashboardOutput DashboardOutput { get; }

        public Dashboard UserDashboard { get; }

        public CustomizableDashboardViewModel(
            DashboardOutput dashboardOutput,
            Dashboard userDashboard)
        {
            DashboardOutput = dashboardOutput;
            UserDashboard = userDashboard;
        }
    }
}