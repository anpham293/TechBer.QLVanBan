using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[Table("CapQuanLies")]
    [Audited]
    public class CapQuanLy : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Ten { get; set; }
		

    }
}