using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[Table("DuAns")]
    public class DuAn : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Descriptions { get; set; }
		

		public virtual int? LoaiDuAnId { get; set; }
		
        [ForeignKey("LoaiDuAnId")]
		public LoaiDuAn LoaiDuAnFk { get; set; }
		
    }
}