
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditDuAnDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Descriptions { get; set; }
		
		
		 public int? LoaiDuAnId { get; set; }
		 public int? ChuongId { get; set; }
		 
		 public int? LoaiKhoanId { get; set; }
		 public string MaDVQHNS { get; set; }
		 public DateTime? NgayBatDau { get; set; }
		 public DateTime? NgayKetThuc { get; set; }
		 public decimal TongMucDauTu { get; set; }
		 public decimal DuToan { get; set; }
    }
}