using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}