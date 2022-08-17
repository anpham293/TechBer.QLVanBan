using Abp.AspNetCore.Mvc.ViewComponents;

namespace TechBer.ChuyenDoiSo.Web.Views
{
    public abstract class ChuyenDoiSoViewComponent : AbpViewComponent
    {
        protected ChuyenDoiSoViewComponent()
        {
            LocalizationSourceName = ChuyenDoiSoConsts.LocalizationSourceName;
        }
    }
}