using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetDuAnThuHoiForViewDto
    {
		public DuAnThuHoiDto DuAnThuHoi { get; set; }

        public decimal TongDuDuAn { get; set; }
        public decimal TongThuDuAn { get; set; }
    }

    public class Data_DanhMuc_ListChiTietThuHoiDto
    {
        public DanhMucThuHoiDto DanhMucThuHoi { get; set; }
        public List<ChiTietThuHoiDto> ListChiTietThuHoi { get; set; }
    }

    public class BaoCaoDuAnThuHoi_ExportToFileDto
    {
        public DuAnThuHoiDto DuAnThuHoi { get; set; }
        public List<Data_DanhMuc_ListChiTietThuHoiDto> ListData { get; set; }
    }
    public class TongHop_BaoCaoDuAnThuHoi_ExportToFileDto
    {
        public List<BaoCaoDuAnThuHoi_ExportToFileDto> listDataDuAn { get; set; }
    }
}