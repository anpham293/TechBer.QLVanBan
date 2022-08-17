using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.Sessions.Dto;

namespace TechBer.ChuyenDoiSo.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}