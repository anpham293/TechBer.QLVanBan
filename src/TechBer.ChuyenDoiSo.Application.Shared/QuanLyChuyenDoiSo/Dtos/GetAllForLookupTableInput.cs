using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}