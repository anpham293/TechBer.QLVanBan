using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class ChuongLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int CapQuanLyFilterId { get; set; }
    }
}