using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Authorization.Users;

namespace TechBer.ChuyenDoiSo.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
