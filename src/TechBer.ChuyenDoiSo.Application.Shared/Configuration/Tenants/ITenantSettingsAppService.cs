using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Configuration.Tenants.Dto;

namespace TechBer.ChuyenDoiSo.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
