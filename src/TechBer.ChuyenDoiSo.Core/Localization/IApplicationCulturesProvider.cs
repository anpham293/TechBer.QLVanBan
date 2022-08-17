using System.Globalization;

namespace TechBer.ChuyenDoiSo.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}