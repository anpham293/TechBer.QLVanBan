namespace TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto
{
    public class NoiDungBaoCaoChinh
    {
        public double ThongTinChung { get; set; }
        public double NhanThucSo { get; set; }
        public double TheCheSo { get; set; }
        public double HaTangSo { get; set; }
        public double NhanLucSo { get; set; }
        public double AnToanThongTinMang { get; set; }
        public double HoatDongChinhQuyenSo { get; set; }
        public double HoatDongKinhTeSo { get; set; }
        public double HoatDongXaHoiSo { get; set; }
        public double DoThiThongMinh { get; set; }
        public double ThongTinVaDuLieuSo { get; set; }

        public void ChiaDeuTheoSoDoiTuong(int soDoiTuong)
        {
            ThongTinChung = ThongTinChung / soDoiTuong;
            NhanThucSo = NhanThucSo / soDoiTuong;
            TheCheSo = TheCheSo / soDoiTuong;
            HaTangSo = HaTangSo / soDoiTuong;
            NhanLucSo = NhanLucSo / soDoiTuong;
            AnToanThongTinMang = AnToanThongTinMang / soDoiTuong;
            HoatDongChinhQuyenSo = HoatDongChinhQuyenSo / soDoiTuong;
            HoatDongKinhTeSo = HoatDongKinhTeSo / soDoiTuong;
            HoatDongXaHoiSo = HoatDongXaHoiSo / soDoiTuong;
            DoThiThongMinh = DoThiThongMinh / soDoiTuong;
            ThongTinVaDuLieuSo = ThongTinVaDuLieuSo / soDoiTuong;
        }
    }
}
