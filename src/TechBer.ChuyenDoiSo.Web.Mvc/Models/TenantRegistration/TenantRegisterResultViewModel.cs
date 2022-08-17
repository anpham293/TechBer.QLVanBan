using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.MultiTenancy.Dto;

namespace TechBer.ChuyenDoiSo.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}