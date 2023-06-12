using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[Table("QuyetDinhs")]
    [Audited]
    public class QuyetDinh : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string So { get; set; }
		
		public virtual string Ten { get; set; }
		
		public virtual DateTime NgayBanHanh { get; set; }
		
		public virtual string FileQuyetDinh { get; set; }
		
		public virtual int TrangThai { get; set; }
		

    }
}