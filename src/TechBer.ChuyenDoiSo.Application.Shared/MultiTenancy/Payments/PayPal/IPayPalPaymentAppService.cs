using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments.PayPal.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
