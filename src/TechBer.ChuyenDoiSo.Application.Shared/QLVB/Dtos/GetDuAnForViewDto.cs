using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetDuAnForViewDto
    {
		public DuAnDto DuAn { get; set; }

		public string LoaiDuAnName { get; set;}
		public string ChuongName { get; set;}
		public string LoaiKhoanName { get; set;}
		
		public LoaiDuAnDto LoaiDuAn { get; set; }
		public long UserId { get; set; }

		public decimal TongSoTienThanhToan { get; set; }
    }
}