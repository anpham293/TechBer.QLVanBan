using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}