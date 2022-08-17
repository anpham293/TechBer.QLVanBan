namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.HostDashboard
{
    public class HostDashboardViewModel
    {
        public int ReportOnLoadDayCount { get; set; } 

        public HostDashboardViewModel(int reportOnLoadDayCount)
        {
            ReportOnLoadDayCount = reportOnLoadDayCount;
        }
    }
}