using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
	[Table("DuAnThuHoies")]
    public class DuAnThuHoi : FullAuditedEntity<long> 
    {

		public virtual string MaDATH { get; set; }
		
		public virtual string Ten { get; set; }
		
		public virtual int NamQuanLy { get; set; }
		
		public virtual DateTime? ThoiHanBaoLanhHopDong { get; set; }
		
		public virtual DateTime? ThoiHanBaoLanhTamUng { get; set; }
		
		public virtual string GhiChu { get; set; }
		
		public virtual int TrangThai { get; set; }
		public virtual int SoDuAn { get; set; }
		

    }
}