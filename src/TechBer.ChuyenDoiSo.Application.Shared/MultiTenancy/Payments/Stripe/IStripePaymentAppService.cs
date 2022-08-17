using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments.Stripe.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}