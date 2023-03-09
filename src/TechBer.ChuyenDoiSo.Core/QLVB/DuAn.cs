using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TechBer.ChuyenDoiSo.QLVB
{
    [Table("DuAns")]
    public class DuAn : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required] public virtual string Name { get; set; }

        public virtual string Descriptions { get; set; }


        public virtual int? LoaiDuAnId { get; set; }

        [ForeignKey("LoaiDuAnId")] public LoaiDuAn LoaiDuAnFk { get; set; }

        public virtual int TrangThai { get; set; }
        
        public virtual int? ChuongId { get; set; }
        public virtual int? LoaiKhoanId { get; set; }
        public virtual string MaDVQHNS { get; set; }
        public virtual DateTime? NgayBatDau { get; set; }
        public virtual DateTime? NgayKetThuc { get; set; }
    }
}