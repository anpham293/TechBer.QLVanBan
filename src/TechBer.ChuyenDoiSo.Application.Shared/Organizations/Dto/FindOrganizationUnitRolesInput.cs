using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}