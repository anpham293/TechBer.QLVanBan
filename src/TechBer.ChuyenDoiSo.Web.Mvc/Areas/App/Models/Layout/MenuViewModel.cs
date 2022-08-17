using Abp.Application.Navigation;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Layout
{
    public class MenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }
}