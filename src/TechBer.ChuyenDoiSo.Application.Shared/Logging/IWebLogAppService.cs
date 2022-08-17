using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Logging.Dto;

namespace TechBer.ChuyenDoiSo.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
