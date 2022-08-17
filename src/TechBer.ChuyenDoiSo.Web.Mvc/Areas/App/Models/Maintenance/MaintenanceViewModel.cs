using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Caching.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}