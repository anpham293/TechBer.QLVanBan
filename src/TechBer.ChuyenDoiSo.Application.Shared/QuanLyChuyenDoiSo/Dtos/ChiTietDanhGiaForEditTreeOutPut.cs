namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class ChiTietDanhGiaForEditTreeOutPut : ChiTietDanhGiaDto
    {
        public string Name { get; set; }

        public string SoThuTu { get; set; }

        public string PhuongThucDanhGia { get; set; }

        public double? DiemToiDa { get; set; }

        public bool IsLeaf { get; set; }
    }
}
