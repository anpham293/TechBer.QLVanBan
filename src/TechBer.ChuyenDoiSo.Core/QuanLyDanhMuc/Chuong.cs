using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[Table("Chuongs")]
    [Audited]
    public class Chuong : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string MaSo { get; set; }
		
		public virtual string Ten { get; set; }
		

		public virtual int? CapQuanLyId { get; set; }
		
        [ForeignKey("CapQuanLyId")]
		public CapQuanLy CapQuanLyFk { get; set; }
		
    }
}