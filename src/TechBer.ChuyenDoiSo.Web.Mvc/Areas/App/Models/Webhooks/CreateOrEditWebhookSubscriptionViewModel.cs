using Abp.Application.Services.Dto;
using Abp.Webhooks;
using TechBer.ChuyenDoiSo.WebHooks.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
