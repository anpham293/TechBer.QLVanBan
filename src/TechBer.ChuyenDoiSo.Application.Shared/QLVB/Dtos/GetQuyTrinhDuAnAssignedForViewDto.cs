namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetQuyTrinhDuAnAssignedForViewDto
    {
		public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }

		public string LoaiDuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}

		public string QuyTrinhDuAnAssignedName { get; set;}

		public string DuAnName { get; set;}


    }
    public class GetQuyTrinhDuAnAssignedForView2Dto
    {
	    public QuyTrinhDuAnAssignedHasMemberCountDto QuyTrinhDuAn { get; set; }

	    public string TieuChiDanhGiaName { get; set;}

	    public string DoiTuongQLDLNongNghiepName { get; set;}
    }
}