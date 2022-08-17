using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[Table("TinhThanhs")]
    [Audited]
    public class TinhThanh : FullAuditedEntity 
    {

		[Required]
		public virtual string Name { get; set; }
		
		[Required]
		public virtual string Ma { get; set; }
		

    }
}