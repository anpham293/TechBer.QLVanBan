using TechBer.ChuyenDoiSo.Editions;
using TechBer.ChuyenDoiSo.Editions.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments;
using TechBer.ChuyenDoiSo.Security;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments.Dto;

namespace TechBer.ChuyenDoiSo.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
