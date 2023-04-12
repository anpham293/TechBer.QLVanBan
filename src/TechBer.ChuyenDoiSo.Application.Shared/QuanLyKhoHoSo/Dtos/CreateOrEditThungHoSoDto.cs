
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class CreateOrEditThungHoSoDto : EntityDto<int?>
    {

		public string MaSo { get; set; }
		
		
		public string Ten { get; set; }
		
		
		public string MoTa { get; set; }
		
		
		public int TrangThai { get; set; }
		
		
		 public int? DayKeId { get; set; }
		 
		 		 public int? DuAnId { get; set; }
		 
		 
    }
}