using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
	[Table("ChiTietThuHoies")]
    [Audited]
    public class ChiTietThuHoi : FullAuditedEntity<long> 
    {

		public virtual decimal Du1 { get; set; }
		
		public virtual decimal Du2 { get; set; }
		
		public virtual decimal Du3 { get; set; }
		
		public virtual decimal Du4 { get; set; }
		
		public virtual decimal Du5 { get; set; }
		
		public virtual decimal Du6 { get; set; }
		
		public virtual decimal Du7 { get; set; }
		
		public virtual decimal Du8 { get; set; }
		
		public virtual decimal Du9 { get; set; }
		
		public virtual decimal Du10 { get; set; }
		
		public virtual decimal Du11 { get; set; }
		
		public virtual decimal Du12 { get; set; }
		
		public virtual decimal Thu1 { get; set; }
		
		public virtual decimal Thu2 { get; set; }
		
		public virtual decimal Thu3 { get; set; }
		
		public virtual decimal Thu4 { get; set; }
		
		public virtual decimal Thu5 { get; set; }
		
		public virtual decimal Thu6 { get; set; }
		
		public virtual decimal Thu7 { get; set; }
		
		public virtual decimal Thu8 { get; set; }
		
		public virtual decimal Thu9 { get; set; }
		
		public virtual decimal Thu10 { get; set; }
		
		public virtual decimal Thu11 { get; set; }
		
		public virtual decimal Thu12 { get; set; }
		
		public virtual string GhiChu { get; set; }
		
		public virtual string Ten { get; set; }
		

		public virtual long? DanhMucThuHoiId { get; set; }
		
        [ForeignKey("DanhMucThuHoiId")]
		public DanhMucThuHoi DanhMucThuHoiFk { get; set; }
		
		public virtual decimal TongDu { get; set; }
		public virtual decimal TongThu { get; set; }
		
		public virtual decimal ThucTe1 { get; set; }
		
		public virtual decimal ThucTe2 { get; set; }
		
		public virtual decimal ThucTe3 { get; set; }
		
		public virtual decimal ThucTe4 { get; set; }
		
		public virtual decimal ThucTe5 { get; set; }
		
		public virtual decimal ThucTe6 { get; set; }
		
		public virtual decimal ThucTe7 { get; set; }
		
		public virtual decimal ThucTe8 { get; set; }
		
		public virtual decimal ThucTe9 { get; set; }
		
		public virtual decimal ThucTe10 { get; set; }
		
		public virtual decimal ThucTe11 { get; set; }
		
		public virtual decimal ThucTe12 { get; set; }
		public virtual decimal TongThucTe { get; set; }
    }
}