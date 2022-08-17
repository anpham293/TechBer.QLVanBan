using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Configuration.Host.Dto;

namespace TechBer.ChuyenDoiSo.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
