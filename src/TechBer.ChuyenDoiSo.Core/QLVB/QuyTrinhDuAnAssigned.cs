using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB;
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
	[Table("QuyTrinhDuAnAssigneds")]
    [Audited]
    public class 
	    QuyTrinhDuAnAssigned : FullAuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Descriptions { get; set; }
		
		public virtual int STT { get; set; }
		
		public virtual int SoVanBanQuyDinh { get; set; }
		
		[StringLength(QuyTrinhDuAnAssignedConsts.MaxMaQuyTrinhLength, MinimumLength = QuyTrinhDuAnAssignedConsts.MinMaQuyTrinhLength)]
		public virtual string MaQuyTrinh { get; set; }
		

		public virtual int? LoaiDuAnId { get; set; }
		
        [ForeignKey("LoaiDuAnId")]
		public LoaiDuAn LoaiDuAnFk { get; set; }
		
		public virtual int? QuyTrinhDuAnId { get; set; }
		
        [ForeignKey("QuyTrinhDuAnId")]
		public QuyTrinhDuAn QuyTrinhDuAnFk { get; set; }
		
		public virtual long? ParentId { get; set; }
		
        [ForeignKey("ParentId")]
		public QuyTrinhDuAnAssigned ParentFk { get; set; }
		
		public virtual int? DuAnId { get; set; }
		
        [ForeignKey("DuAnId")]
		public DuAn DuAnFk { get; set; }
		
		public virtual int TrangThai { get; set; }
    }
}