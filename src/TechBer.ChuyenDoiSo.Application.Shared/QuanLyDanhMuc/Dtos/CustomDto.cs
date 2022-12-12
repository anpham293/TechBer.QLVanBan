using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class BaoCaoNopHoSoTrongThangTheoDuAnFilterDto
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
    }
    public class BaoCaoHoSoTheoDuAnFilterDto
    {
        public string MaDuAn { get; set; }
        public string TenDuAn { get; set; }
        public string TenHoSo { get; set; }
        public string NgayQuyetDinh { get; set; }
    }
    
    public class BaoCaoNopHoSoTrongThangTheoDuAnDto
    {
        public LoaiDuAnDto LoaiDuAn { get; set; }
        public DuAnDto DuAn { get; set; }
        public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }
        public VanBanDuAnDto VanBanDuAn { get; set; }
        
        public string TenNguoiNop { get; set; }
    }
    public class BaoCaoHoSoDuAnDto
    {
        public LoaiDuAnDto LoaiDuAn { get; set; }
        public DuAnDto DuAn { get; set; }
        public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }
        public VanBanDuAnDto VanBanDuAn { get; set; }
        
        public string TenNguoiNop { get; set; }
    }
    
}