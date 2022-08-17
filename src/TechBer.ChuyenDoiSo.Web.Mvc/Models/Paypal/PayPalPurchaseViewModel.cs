using TechBer.ChuyenDoiSo.MultiTenancy.Payments.Paypal;

namespace TechBer.ChuyenDoiSo.Web.Models.Paypal
{
    public class PayPalPurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PayPalPaymentGatewayConfiguration Configuration { get; set; }
    }
}
