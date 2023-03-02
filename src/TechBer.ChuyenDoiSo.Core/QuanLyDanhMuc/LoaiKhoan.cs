using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[Table("LoaiKhoans")]
    [Audited]
    public class LoaiKhoan : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string MaSo { get; set; }
		
		public virtual string Ten { get; set; }
		
		public virtual string GhiChu { get; set; }
		

    }
}