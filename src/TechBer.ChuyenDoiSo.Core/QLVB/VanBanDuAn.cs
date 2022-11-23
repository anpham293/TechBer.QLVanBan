using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QLVB
{
    [Table("VanBanDuAns")]
    [Audited]
    public class VanBanDuAn : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(VanBanDuAnConsts.MaxNameLength, MinimumLength = VanBanDuAnConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(VanBanDuAnConsts.MaxKyHieuVanBanLength, MinimumLength = VanBanDuAnConsts.MinKyHieuVanBanLength)]
        public virtual string KyHieuVanBan { get; set; }

        public virtual DateTime NgayBanHanh { get; set; }

        public virtual string FileVanBan { get; set; }


        public virtual int? DuAnId { get; set; }

        [ForeignKey("DuAnId")] public DuAn DuAnFk { get; set; }

        public virtual long? QuyTrinhDuAnAssignedId { get; set; }

        [ForeignKey("QuyTrinhDuAnAssignedId")] public QuyTrinhDuAnAssigned QuyTrinhDuAnAssignedFk { get; set; }

        public virtual string ViTriLuuTru { get; set; }
        public virtual DateTime? LastFileVanBanTime { get; set; }
        public virtual long? NguoiNopHoSoId { get; set; }
        public virtual decimal SoTienThanhToan { get; set; }
    }
}