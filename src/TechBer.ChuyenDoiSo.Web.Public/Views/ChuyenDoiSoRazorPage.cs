using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace TechBer.ChuyenDoiSo.Web.Public.Views
{
    public abstract class ChuyenDoiSoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ChuyenDoiSoRazorPage()
        {
            LocalizationSourceName = ChuyenDoiSoConsts.LocalizationSourceName;
        }
    }
}
