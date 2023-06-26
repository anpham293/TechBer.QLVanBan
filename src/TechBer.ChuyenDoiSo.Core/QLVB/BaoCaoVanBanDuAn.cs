using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[Table("BaoCaoVanBanDuAns")]
    [Audited]
    public class BaoCaoVanBanDuAn : FullAuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string NoiDungCongViec { get; set; }
		
		public virtual string MoTaChiTiet { get; set; }
		
		public virtual string FileBaoCao { get; set; }
		

		public virtual int? VanBanDuAnId { get; set; }
		
        [ForeignKey("VanBanDuAnId")]
		public VanBanDuAn VanBanDuAnFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}