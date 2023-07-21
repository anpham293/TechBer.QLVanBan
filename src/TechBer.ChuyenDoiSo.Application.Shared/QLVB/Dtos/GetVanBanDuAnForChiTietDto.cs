using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetVanBanDuAnForChiTietDto
    {
        public VanBanDuAnDto VanBanDuAn { get; set; }
        public DuAnDto DuAn { get; set; }
        
        public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }
        
        public QuyetDinhDto QuyetDinh { get; set; }
        
        public ThungHoSoDto ThungHoSo { get; set; }
        public long Session_UserId { get; set; }
    }
}