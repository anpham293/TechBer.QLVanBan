using Abp.AspNetCore.Mvc.Views;

namespace TechBer.ChuyenDoiSo.Web.Views
{
    public abstract class ChuyenDoiSoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected ChuyenDoiSoRazorPage()
        {
            LocalizationSourceName = ChuyenDoiSoConsts.LocalizationSourceName;
        }
    }
}
