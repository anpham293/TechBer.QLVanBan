namespace TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto
{
    public class BaoCaoTongHopOutput
    {
        public BaoCaoTongHopOutput()
        {
            DiemTuDanhGia = new NoiDungBaoCaoChinh();
            DiemHoiDongThamDinh = new NoiDungBaoCaoChinh();
            DiemDatDuoc = new NoiDungBaoCaoChinh();
        }
        public NoiDungBaoCaoChinh DiemTuDanhGia { get; set; }
        public NoiDungBaoCaoChinh DiemHoiDongThamDinh { get; set; }
        public NoiDungBaoCaoChinh DiemDatDuoc { get; set; }

        public void ChiaDeuTheoSoDoiTuong(int soDoiTuong)
        {
            DiemTuDanhGia.ChiaDeuTheoSoDoiTuong(soDoiTuong);
            DiemHoiDongThamDinh.ChiaDeuTheoSoDoiTuong(soDoiTuong);
            DiemDatDuoc.ChiaDeuTheoSoDoiTuong(soDoiTuong);
        }
    }
}
