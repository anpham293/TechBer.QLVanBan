using TechBer.ChuyenDoiSo.MultiTenancy.Payments;

namespace TechBer.ChuyenDoiSo.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}