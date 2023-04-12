using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo
{
	[Table("ThungHoSos")]
    [Audited]
    public class ThungHoSo : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string MaSo { get; set; }
		
		public virtual string Ten { get; set; }
		
		public virtual string MoTa { get; set; }
		
		public virtual int TrangThai { get; set; }
		public virtual string QrString { get; set; }
		

		public virtual int? DayKeId { get; set; }
		
        [ForeignKey("DayKeId")]
		public DayKe DayKeFk { get; set; }
		
		public virtual int? DuAnId { get; set; }
		
        [ForeignKey("DuAnId")]
		public DuAn DuAnFk { get; set; }
		
    }
}