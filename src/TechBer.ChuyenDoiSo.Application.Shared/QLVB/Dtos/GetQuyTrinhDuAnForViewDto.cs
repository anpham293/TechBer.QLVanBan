namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetQuyTrinhDuAnForViewDto
    {
		public QuyTrinhDuAnDto QuyTrinhDuAn { get; set; }

		public string LoaiDuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}


    }
    public class GetQuyTrinhDuAnForView2Dto
    {
	    public QuyTrinhDuAnHasMemberCountDto QuyTrinhDuAn { get; set; }

	    public string TieuChiDanhGiaName { get; set;}

	    public string DoiTuongQLDLNongNghiepName { get; set;}


    }
}