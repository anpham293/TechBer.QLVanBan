using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[Table("QuanHuyens")]
    [Audited]
    public class QuanHuyen : FullAuditedEntity 
    {

		[Required]
		public virtual string Name { get; set; }
		
		[Required]
		public virtual string Ma { get; set; }
		

		public virtual int TinhThanhId { get; set; }
		
        [ForeignKey("TinhThanhId")]
		public TinhThanh TinhThanhFk { get; set; }
		
    }
}