namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetChuyenHoSoGiayForViewDto
    {
		public ChuyenHoSoGiayDto ChuyenHoSoGiay { get; set; }

		public string VanBanDuAnName { get; set;}

		public string UserName { get; set;}

		public string TenNguoiChuyen { get; set; }
		public string TenNguoiNhan { get; set; }
		public int UserId { get; set; }
		public string QuyTrinhDuAnName { get; set; }
		public string DuAnName { get; set; }
    }
}