using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetAllDuAnThuHoiesForExcelInput
    {
		public string Filter { get; set; }

		public string MaDATHFilter { get; set; }

		public string TenFilter { get; set; }

		public int? MaxNamQuanLyFilter { get; set; }
		public int? MinNamQuanLyFilter { get; set; }

		public DateTime? MaxThoiHanBaoLanhHopDongFilter { get; set; }
		public DateTime? MinThoiHanBaoLanhHopDongFilter { get; set; }

		public DateTime? MaxThoiHanBaoLanhTamUngFilter { get; set; }
		public DateTime? MinThoiHanBaoLanhTamUngFilter { get; set; }

		public string GhiChuFilter { get; set; }

		public int? MaxTrangThaiFilter { get; set; }
		public int? MinTrangThaiFilter { get; set; }



    }
}