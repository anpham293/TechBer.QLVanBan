using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetAllCapQuanLiesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TenFilter { get; set; }



    }
}