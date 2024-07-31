using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
	[Table("DanhMucThuHoies")]
    [Audited]
    public class DanhMucThuHoi : FullAuditedEntity<long> 
    {

		public virtual string Stt { get; set; }
		
		public virtual string Ten { get; set; }
		
		public virtual string GhiChu { get; set; }
		
		public virtual int Type { get; set; }
		

		public virtual long? DuAnThuHoiId { get; set; }
		
        [ForeignKey("DuAnThuHoiId")]
		public DuAnThuHoi DuAnThuHoiFk { get; set; }
		
    }
}