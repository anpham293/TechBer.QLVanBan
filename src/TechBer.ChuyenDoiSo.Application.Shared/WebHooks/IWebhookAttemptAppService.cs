using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.WebHooks.Dto;

namespace TechBer.ChuyenDoiSo.WebHooks
{
    public interface IWebhookAttemptAppService
    {
        Task<PagedResultDto<GetAllSendAttemptsOutput>> GetAllSendAttempts(GetAllSendAttemptsInput input);

        Task<ListResultDto<GetAllSendAttemptsOfWebhookEventOutput>> GetAllSendAttemptsOfWebhookEvent(GetAllSendAttemptsOfWebhookEventInput input);
    }
}
