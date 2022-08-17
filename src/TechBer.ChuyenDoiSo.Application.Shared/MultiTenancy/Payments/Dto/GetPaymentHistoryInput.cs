using Abp.Runtime.Validation;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Payments.Dto
{
    public class GetPaymentHistoryInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }

            Sorting = Sorting.Replace("editionDisplayName", "Edition.DisplayName");
        }
    }
}
