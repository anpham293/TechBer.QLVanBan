using System.Threading.Tasks;
using Abp.Dependency;

namespace TechBer.ChuyenDoiSo.UiCustomization
{
    public interface IUiThemeCustomizerFactory : ISingletonDependency
    {
        Task<IUiCustomizer> GetCurrentUiCustomizer();

        IUiCustomizer GetUiCustomizer(string theme);
    }
}