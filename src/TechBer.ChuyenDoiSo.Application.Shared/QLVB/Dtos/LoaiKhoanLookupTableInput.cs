using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class LoaiKhoanLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}