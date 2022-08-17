using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Configuration.Tenants.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}