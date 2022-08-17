using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetAllChiTietDanhGiasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DescriptionFilter { get; set; }

		public string SoLieuKeKhaiFilter { get; set; }

		public double? MaxDiemTuDanhGiaFilter { get; set; }
		public double? MinDiemTuDanhGiaFilter { get; set; }

		public double? MaxDiemHoiDongThamDinhFilter { get; set; }
		public double? MinDiemHoiDongThamDinhFilter { get; set; }

		public double? MaxDiemDatDuocFilter { get; set; }
		public double? MinDiemDatDuocFilter { get; set; }


		 public string TieuChiDanhGiaNameFilter { get; set; }

		 		 public string DoiTuongChuyenDoiSoNameFilter { get; set; }

		 
    }
}