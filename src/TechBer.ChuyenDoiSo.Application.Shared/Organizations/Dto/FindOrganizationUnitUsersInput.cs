using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
