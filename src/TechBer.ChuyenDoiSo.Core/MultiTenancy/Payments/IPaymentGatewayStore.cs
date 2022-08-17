using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
