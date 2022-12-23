using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditQuyTrinhDuAnAssignedDto : EntityDto<long?>
    {
        [Required] public string Name { get; set; }


        public string Descriptions { get; set; }


        public int STT { get; set; }


        public int SoVanBanQuyDinh { get; set; }


        [StringLength(QuyTrinhDuAnAssignedConsts.MaxMaQuyTrinhLength,
            MinimumLength = QuyTrinhDuAnAssignedConsts.MinMaQuyTrinhLength)]
        public string MaQuyTrinh { get; set; }


        public int? LoaiDuAnId { get; set; }

        public int? QuyTrinhDuAnId { get; set; }

        public long? ParentId { get; set; }

        public int? DuAnId { get; set; }
    }
    
    public class XuLyHoSoInputDto
    {
        public int TypeDuyetHoSo { get; set; }
        public string XuLyCuaLanhDao { get; set; }
        public long NguoiGuiId { get; set; }
        public long keToanTiepNhanId { get; set; }
        public long NguoiDuyetId { get; set; }
        public int vanBanDuAnId { get; set; }
    }
}