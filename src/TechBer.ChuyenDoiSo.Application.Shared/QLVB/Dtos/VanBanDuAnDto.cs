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
        public DateTime? LastFileVanBanTime { get; set; }
        
        public long? NguoiNopHoSoId { get; set; }
        public int TrangThaiChuyenDuyetHoSo { get; set; }
        public long? NguoiGuiId { get; set; }
        public DateTime? NgayGui { get; set; }
        public long? NguoiDuyetId { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public long? KeToanTiepNhanId { get; set; }
        public string XuLyCuaLanhDao { get; set; }
        public int SoLuongVanBanGiay { get; set; }
    }
}