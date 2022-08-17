using System.Collections.Generic;
using System.Linq;
using TechBer.ChuyenDoiSo.Configuration.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.UiCustomization
{
    public class UiCustomizationViewModel
    {
        public string Theme { get; set; }

        public List<ThemeSettingsDto> Settings { get; set; }

        public bool HasUiCustomizationPagePermission { get; set; }

        public ThemeSettingsDto GetThemeSettings(string themeName)
        {
            return Settings.First(s => s.Theme == themeName);
        }
    }
}
