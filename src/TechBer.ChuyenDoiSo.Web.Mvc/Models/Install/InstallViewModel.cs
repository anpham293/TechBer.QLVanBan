using System.Collections.Generic;
using Abp.Localization;
using TechBer.ChuyenDoiSo.Install.Dto;

namespace TechBer.ChuyenDoiSo.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
