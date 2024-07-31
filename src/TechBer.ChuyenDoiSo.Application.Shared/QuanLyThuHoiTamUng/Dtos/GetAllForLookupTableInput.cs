using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}