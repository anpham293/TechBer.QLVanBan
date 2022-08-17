using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.Organizations.Dto;

namespace TechBer.ChuyenDoiSo.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}