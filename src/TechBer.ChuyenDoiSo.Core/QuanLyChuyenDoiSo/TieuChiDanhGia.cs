using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
	[Table("TieuChiDanhGias")]
    [Audited]
    public class TieuChiDanhGia : FullAuditedEntity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual double DiemToiDa { get; set; }
		
		public virtual string PhuongThucDanhGia { get; set; }
		
		public virtual string TaiLieuGiaiTrinh { get; set; }
		
		public virtual string GhiChu { get; set; }
		
		public virtual int LoaiPhuLuc { get; set; }

		public virtual int? SapXep { get; set; }

		/// <summary>
		/// Thuộc tính này sử dụng ẩn phục vụ cho thống kê báo cáo
		/// </summary>
		public virtual int? PhanNhomLevel1 { get; set; }

		/// <summary>
		/// Độ sâu của tiêu chí
		/// </summary>
		public virtual int? DoSau { get; set; }
		
		[Required]
		public virtual string SoThuTu { get; set; }
		

		public virtual int? ParentId { get; set; }
		
        [ForeignKey("ParentId")]
		public TieuChiDanhGia ParentFk { get; set; }
		
    }
}