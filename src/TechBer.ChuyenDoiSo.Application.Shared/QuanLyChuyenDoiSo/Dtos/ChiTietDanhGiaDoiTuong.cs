
namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class ChiTietDanhGiaDoiTuong
    {
		public int Id { get; set; }

		public string Description { get; set; }

		public string SoLieuKeKhai { get; set; }

		public double? DiemTuDanhGia { get; set; }

		public double? DiemHoiDongThamDinh { get; set; }

		public double? DiemDatDuoc { get; set; }

		public double? DiemToiDa { get; set; }

		public int? ParentId { get; set; }

		public int TieuChiId { get; set; }

		public string TenTieuChi { get; set; }

		public int? ParentChiTietId { get; set; }

		public string SoThuTu { get; set; }

		public int? SapXep { get; set; }

		public bool? IsHoiDongThamDinh { get; set; }

		public bool? IsTuDanhGia { get; set; }

		public bool? IsLeaf { get; set; }
	}
}
