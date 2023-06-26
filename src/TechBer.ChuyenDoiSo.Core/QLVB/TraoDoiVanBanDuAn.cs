using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[Table("TraoDoiVanBanDuAns")]
    [Audited]
    public class TraoDoiVanBanDuAn : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual DateTime NgayGui { get; set; }
		
		public virtual string NoiDung { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? VanBanDuAnId { get; set; }
		
        [ForeignKey("VanBanDuAnId")]
		public VanBanDuAn VanBanDuAnFk { get; set; }
		
    }
}