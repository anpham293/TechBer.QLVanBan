using TechBer.ChuyenDoiSo.Editions.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < ChuyenDoiSoConsts.MinimumUpgradePaymentAmount;
        }
    }
}
