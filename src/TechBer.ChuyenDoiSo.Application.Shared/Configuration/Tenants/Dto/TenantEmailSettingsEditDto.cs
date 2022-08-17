using Abp.Auditing;
using TechBer.ChuyenDoiSo.Configuration.Dto;

namespace TechBer.ChuyenDoiSo.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}