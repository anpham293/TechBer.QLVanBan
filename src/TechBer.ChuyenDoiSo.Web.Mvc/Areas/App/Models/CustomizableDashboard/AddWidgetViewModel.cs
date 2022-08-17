using System.Collections.Generic;
using TechBer.ChuyenDoiSo.DashboardCustomization.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
