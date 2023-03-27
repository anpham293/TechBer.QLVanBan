
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditChuyenHoSoGiayDto : EntityDto<int?>
    {

		public int NguoiChuyenId { get; set; }
		
		
		public DateTime? ThoiGianChuyen { get; set; }
		
		
		public int SoLuong { get; set; }
		
		
		public int TrangThai { get; set; }
		
		
		public DateTime? ThoiGianNhan { get; set; }
		
		
		 public int? VanBanDuAnId { get; set; }
		 
		 		 public long? NguoiNhanId { get; set; }
		 
		 
    }
}