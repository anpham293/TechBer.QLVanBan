using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }

        public bool ExcludeCurrentUser { get; set; }
    }
}