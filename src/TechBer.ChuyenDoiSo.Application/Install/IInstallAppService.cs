using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Install.Dto;

namespace TechBer.ChuyenDoiSo.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}