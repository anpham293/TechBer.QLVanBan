using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
	[Table("ChiTietDanhGias")]
    [Audited]
    public class ChiTietDanhGia : FullAuditedEntity , IMustHaveTenant
    {
		public int TenantId { get; set; }
			
		public virtual string Description { get; set; }
		
		public virtual string SoLieuKeKhai { get; set; }
		
		public virtual double? DiemTuDanhGia { get; set; }
		
		public virtual double? DiemHoiDongThamDinh { get; set; }
		
		public virtual double? DiemDatDuoc { get; set; }
		
		public virtual bool? IsTuDanhGia { get; set; }

		public virtual bool? IsHoiDongThamDinh { get; set; }

		public virtual int TieuChiDanhGiaId { get; set; }
		
        [ForeignKey("TieuChiDanhGiaId")]
		public TieuChiDanhGia TieuChiDanhGiaFk { get; set; }
		
		public virtual int DoiTuongChuyenDoiSoId { get; set; }
		
        [ForeignKey("DoiTuongChuyenDoiSoId")]
		public DoiTuongChuyenDoiSo DoiTuongChuyenDoiSoFk { get; set; }
		
    }
}