namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetDanhMucThuHoiForViewDto
    {
		public DanhMucThuHoiDto DanhMucThuHoi { get; set; }

		public string DuAnThuHoiMaDATH { get; set;}
		
		public decimal TongDuDanhMuc { get; set; }
		public decimal TongThuDanhMuc { get; set; }

    }
}