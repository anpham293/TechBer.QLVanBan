using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.MultiTenancy.Dto;

namespace TechBer.ChuyenDoiSo.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
