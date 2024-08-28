
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class ChiTietThuHoiDto : EntityDto<long>
    {
		public decimal Du1 { get; set; }

		public decimal Du2 { get; set; }

		public decimal Du3 { get; set; }

		public decimal Du4 { get; set; }

		public decimal Du5 { get; set; }

		public decimal Du6 { get; set; }

		public decimal Du7 { get; set; }

		public decimal Du8 { get; set; }

		public decimal Du9 { get; set; }

		public decimal Du10 { get; set; }

		public decimal Du11 { get; set; }

		public decimal Du12 { get; set; }

		public decimal Thu1 { get; set; }

		public decimal Thu2 { get; set; }

		public decimal Thu3 { get; set; }

		public decimal Thu4 { get; set; }

		public decimal Thu5 { get; set; }

		public decimal Thu6 { get; set; }

		public decimal Thu7 { get; set; }

		public decimal Thu8 { get; set; }

		public decimal Thu9 { get; set; }

		public decimal Thu10 { get; set; }

		public decimal Thu11 { get; set; }

		public decimal Thu12 { get; set; }

		public string GhiChu { get; set; }

		public string Ten { get; set; }


		 public long? DanhMucThuHoiId { get; set; }

		 public decimal TongThu { get; set; }
		 public decimal TongDu { get; set; }
		 public decimal ThucTe1 { get; set; }
		
		 public decimal ThucTe2 { get; set; }
		
		 public decimal ThucTe3 { get; set; }
		
		 public decimal ThucTe4 { get; set; }
		
		 public decimal ThucTe5 { get; set; }
		
		 public decimal ThucTe6 { get; set; }
		
		 public decimal ThucTe7 { get; set; }
		
		 public decimal ThucTe8 { get; set; }
		
		 public decimal ThucTe9 { get; set; }
		
		 public decimal ThucTe10 { get; set; }
		
		 public decimal ThucTe11 { get; set; }
		
		 public decimal ThucTe12 { get; set; }
		 public decimal TongThucTe { get; set; }
    }
}