using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class VanBanDuAnDto : EntityDto
    {
        public string Name { get; set; }

        public string KyHieuVanBan { get; set; }

        public DateTime NgayBanHanh { get; set; }

        public string FileVanBan { get; set; }


        public int? DuAnId { get; set; }

        public int? QuyTrinhDuAnId { get; set; }
        
        public string ViTriLuuTru { get; set; }

        public DateTime? LastFileVanBanTime { get; set; }
        
        public long? NguoiNopHoSoId { get; set; }
        public int TrangThaiNhanHoSoGiay { get; set; }
        public string TenNguoiGiaoHoSo { get; set; }
        public DateTime? ThoiGianNhanHoSoGiay { get; set; }
    }
}