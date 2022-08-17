﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Layout;
using TechBer.ChuyenDoiSo.Web.Session;
using TechBer.ChuyenDoiSo.Web.Views;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Views.Shared.Components.AppTheme6Brand
{
    public class AppTheme6BrandViewComponent : ChuyenDoiSoViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme6BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
