using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard.Dto
{
    public class BaoCaoDuAnOutput
    {
        public int SoDuAn { get; set; }
        public int SoHoSoDangChoDuyet { get; set; }
    }

    public class ChiTietBaoCaoLoaiDuAn
    {
        public string TenLoaiDuAn { get; set; }
        public int LoaiDuAn_SoDuAn { get; set; }
    }
    public class BaoCaoLoaiDuAnOutput
    {
        public List<ChiTietBaoCaoLoaiDuAn> listBaoCaoLoaiDuAns { get; set; }
    }
}