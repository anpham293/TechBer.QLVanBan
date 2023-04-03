namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetVanBanDuAnForViewDto
    {
		public VanBanDuAnDto VanBanDuAn { get; set; }

		public string DuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}

		public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }
		public string TenNguoiGui { get; set; }
		public string TenNguoiDuyet { get; set; }
    }
}