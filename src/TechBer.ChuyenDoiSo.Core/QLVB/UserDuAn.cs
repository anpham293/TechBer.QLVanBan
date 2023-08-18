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
	[Table("UserDuAns")]
    [Audited]
    public class UserDuAn : FullAuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual int TrangThai { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? DuAnId { get; set; }
		
        [ForeignKey("DuAnId")]
		public DuAn DuAnFk { get; set; }
		
    }
}