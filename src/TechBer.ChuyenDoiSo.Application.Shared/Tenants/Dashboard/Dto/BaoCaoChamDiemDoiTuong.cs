namespace TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto
{
    public class BaoCaoChamDiemDoiTuong
    {
        public BaoCaoChamDiemDoiTuong()
        {
            DiemTuDanhGia = new NoiDungBaoCaoChinh();
            DiemHoiDongThamDinh = new NoiDungBaoCaoChinh();
            DiemDatDuoc = new NoiDungBaoCaoChinh();
        }

        public NoiDungBaoCaoChinh DiemTuDanhGia { get; set; }
        public NoiDungBaoCaoChinh DiemHoiDongThamDinh { get; set; }
        public NoiDungBaoCaoChinh DiemDatDuoc { get; set; }
    }
}
