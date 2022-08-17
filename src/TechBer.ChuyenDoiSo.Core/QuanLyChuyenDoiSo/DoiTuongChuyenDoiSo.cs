using TechBer.ChuyenDoiSo.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
	[Table("DoiTuongChuyenDoiSos")]
    [Audited]
    public class DoiTuongChuyenDoiSo : FullAuditedEntity , IMustHaveTenant
    {
		public int TenantId { get; set; }

		[Required]
		public virtual string Name { get; set; }
		
		public virtual int Type { get; set; }
		
		public virtual double? TongDiemTuDanhGia { get; set; }

		public virtual double? TongDiemHoiDongThamDinh { get; set; }

		public virtual double? TongDiemDatDuoc { get; set; }

		public virtual long? UserId { get; set; }

		/// <summary>
		/// Cờ trạng thái chấm điểm: tự đánh giá, 
		/// </summary>
		public virtual int ChamDiemFlag { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}