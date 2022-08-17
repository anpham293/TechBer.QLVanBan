using System.Threading.Tasks;
using Abp.Webhooks;

namespace TechBer.ChuyenDoiSo.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
