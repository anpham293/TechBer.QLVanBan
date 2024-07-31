using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetAllDanhMucThuHoiesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string SttFilter { get; set; }

		public string TenFilter { get; set; }

		public string GhiChuFilter { get; set; }

		public int? MaxTypeFilter { get; set; }
		public int? MinTypeFilter { get; set; }


		 public string DuAnThuHoiMaDATHFilter { get; set; }

		 
    }
}