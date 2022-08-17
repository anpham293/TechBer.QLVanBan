using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.MultiTenancy;
using TechBer.ChuyenDoiSo.MultiTenancy.Dto;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Common;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}