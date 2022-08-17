using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.CayTieuChi
{
    public class EditTieuChiModalViewModel
    {
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }


		[Required]
		public string SoThuTu { get; set; }


		public double DiemToiDa { get; set; }


		public string PhuongThucDanhGia { get; set; }


		public string TaiLieuGiaiTrinh { get; set; }


		public string GhiChu { get; set; }


		public int LoaiPhuLuc { get; set; }

		public int? PhanNhomLevel1 { get; set; }

		public int? DoSau { get; set; }

		public int? ParentId { get; set; }

		public int? SapXep { get; set; }
	}
}
