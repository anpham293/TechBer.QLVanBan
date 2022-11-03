using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyTrinhDuAnAssigneds
{
    public class ChuyenDuyetHoSoModalViewModel
    {
        public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }
        public DuAnDto DuAn { get; set; }
        public int SoLuongVanBan { get; set; }
        public List<CommonLookupTableDto> ListKeToanTiepNhan { get; set; }
        public string TenNguoiGiao { get; set; }
        public string NgayGuiPhieu { get; set; }
        public long? KeToanTiepNhanId { get; set; }
        public long NguoiGuiId { get; set; }
        public int TypeDuyetHoSo { get; set; }
    }

    public class ChuyenDuyetHoSoModalInput
    {
        public int QuyTrinhDuAnAssignedId { get; set; }
        public int TypeDuyetHoSo { get; set; }
    }

    
}