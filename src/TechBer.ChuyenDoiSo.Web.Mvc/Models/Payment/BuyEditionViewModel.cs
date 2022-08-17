using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Editions;
using TechBer.ChuyenDoiSo.Editions.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments.Dto;

namespace TechBer.ChuyenDoiSo.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
