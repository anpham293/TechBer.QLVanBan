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
	[Table("QuyTrinhDuAns")]
    [Audited]
    public class QuyTrinhDuAn : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Descriptions { get; set; }
		
		public virtual int STT { get; set; }
		
		[StringLength(QuyTrinhDuAnConsts.MaxMaQuyTrinhLength, MinimumLength = QuyTrinhDuAnConsts.MinMaQuyTrinhLength)]
		public virtual string MaQuyTrinh { get; set; }
		
		public virtual string GhiChu { get; set; }
		

		public virtual int? LoaiDuAnId { get; set; }
		
        [ForeignKey("LoaiDuAnId")]
		public LoaiDuAn LoaiDuAnFk { get; set; }
		
		public virtual int? ParentId { get; set; }
		
        [ForeignKey("ParentId")]
		public QuyTrinhDuAn ParentFk { get; set; }
		
    }
}