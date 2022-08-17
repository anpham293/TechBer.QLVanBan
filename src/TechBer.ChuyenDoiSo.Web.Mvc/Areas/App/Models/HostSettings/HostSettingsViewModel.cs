using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Configuration.Host.Dto;
using TechBer.ChuyenDoiSo.Editions.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}