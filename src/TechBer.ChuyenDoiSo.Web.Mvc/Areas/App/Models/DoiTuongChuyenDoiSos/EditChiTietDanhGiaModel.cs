using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DoiTuongChuyenDoiSos
{
    public class EditChiTietDanhGiaModel : ChiTietDanhGiaDto
    {
        public string Name { get; set; }

        public string SoThuTu { get; set; }

        public string PhuongThucDanhGia { get; set; }

        public double? DiemToiDa { get; set; }
    }
}
