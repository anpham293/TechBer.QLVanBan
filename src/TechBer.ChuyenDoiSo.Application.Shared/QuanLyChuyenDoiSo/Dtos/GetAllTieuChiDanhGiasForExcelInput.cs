using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetAllTieuChiDanhGiasForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int? MaxDiemToiDaFilter { get; set; }
		public int? MinDiemToiDaFilter { get; set; }

		public string PhuongThucDanhGiaFilter { get; set; }

		public string TaiLieuGiaiTrinhFilter { get; set; }

		public string GhiChuFilter { get; set; }

		public int? MaxLoaiPhuLucFilter { get; set; }
		public int? MinLoaiPhuLucFilter { get; set; }


		 public string TieuChiDanhGiaNameFilter { get; set; }

		 
    }
}