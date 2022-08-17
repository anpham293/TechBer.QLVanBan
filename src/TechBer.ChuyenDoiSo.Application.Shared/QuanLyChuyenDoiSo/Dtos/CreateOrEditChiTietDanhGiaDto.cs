using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class CreateOrEditChiTietDanhGiaDto : EntityDto<int?>
    {
		public string Description { get; set; }
		
		public string SoLieuKeKhai { get; set; }
		
		public double? DiemTuDanhGia { get; set; }
		
		public double? DiemHoiDongThamDinh { get; set; }
		
		public double? DiemDatDuoc { get; set; }

		public int TieuChiDanhGiaId { get; set; }
		 
		public int DoiTuongChuyenDoiSoId { get; set; }
    }
}