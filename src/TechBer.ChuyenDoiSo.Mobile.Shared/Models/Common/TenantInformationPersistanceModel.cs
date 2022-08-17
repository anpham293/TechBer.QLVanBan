using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.ApiClient;

namespace TechBer.ChuyenDoiSo.Models.Common
{
    [AutoMapFrom(typeof(TenantInformation)),
     AutoMapTo(typeof(TenantInformation))]
    public class TenantInformationPersistanceModel
    {
        public string TenancyName { get; set; }

        public int TenantId { get; set; }
    }
}