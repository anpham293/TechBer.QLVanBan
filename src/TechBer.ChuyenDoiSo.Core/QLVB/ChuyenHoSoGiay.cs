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
	[Table("ChuyenHoSoGiaies")]
    [Audited]
    public class ChuyenHoSoGiay : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual int NguoiChuyenId { get; set; }
		
		public virtual DateTime? ThoiGianChuyen { get; set; }
		
		public virtual int SoLuong { get; set; }
		
		public virtual int TrangThai { get; set; }
		
		public virtual DateTime? ThoiGianNhan { get; set; }
		

		public virtual int? VanBanDuAnId { get; set; }
		
        [ForeignKey("VanBanDuAnId")]
		public VanBanDuAn VanBanDuAnFk { get; set; }
		
		public virtual long? NguoiNhanId { get; set; }
		
        [ForeignKey("NguoiNhanId")]
		public User NguoiNhanFk { get; set; }
		
    }
}