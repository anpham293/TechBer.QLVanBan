using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Layout;
using TechBer.ChuyenDoiSo.Web.Session;
using TechBer.ChuyenDoiSo.Web.Views;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Views.Shared.Components.AppTheme11Footer
{
    public class AppTheme11FooterViewComponent : ChuyenDoiSoViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme11FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
