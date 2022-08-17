namespace TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto
{
    public class DiemCuaTieuChiOutput
    {
        public DiemCuaTieuChiOutput()
        {
            DiemTuDanhGia = 0;
            DiemHoiDongThamDinh = 0;
            DiemDatDuoc = 0;
            DoSau = 0;
        }

        public string TenTieuChi { get; set; }

        public int? DoSau { get; set; }

        public double? DiemTuDanhGia { get; set; }

        public double? DiemHoiDongThamDinh { get; set; }

        public double? DiemDatDuoc { get; set; }
    }
}
