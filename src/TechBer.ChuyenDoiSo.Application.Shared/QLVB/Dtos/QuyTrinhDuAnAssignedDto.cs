using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class QuyTrinhDuAnAssignedDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Descriptions { get; set; }

        public int STT { get; set; }

        public int SoVanBanQuyDinh { get; set; }

        public string MaQuyTrinh { get; set; }


        public int? LoaiDuAnId { get; set; }

        public int? QuyTrinhDuAnId { get; set; }

        public long? ParentId { get; set; }

        public int? DuAnId { get; set; }
        
        public int TrangThai { get; set; }
        public long? NguoiGuiId { get; set; }
        public DateTime? NgayGui { get; set; }
        public long? NguoiDuyetId { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public long? KeToanTiepNhanId { get; set; }
        public string XuLyCuaLanhDao { get; set; }
    }

    public class QuyTrinhDuAnAssignedHasMemberCountDto : QuyTrinhDuAnAssignedDto
    {
        public int MemberCount { get; set; }
        public int Mode { get; set; }
        public int TongSoHoSo { get; set; }
        public int TongSoHoSoDaCoFile { get; set; }
    }

    public class ChuyenDuyetHoSoDto
    {
        public DuAnDto DuAn { get; set; }
        public QuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }
    }
    
    public class CommonLookupTableDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }
}