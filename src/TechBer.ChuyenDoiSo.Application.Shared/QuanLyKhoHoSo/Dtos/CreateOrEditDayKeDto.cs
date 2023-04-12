
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class CreateOrEditDayKeDto : EntityDto<int?>
    {

		public string MaSo { get; set; }
		
		
		public string Ten { get; set; }
		
		
		 public int? PhongKhoId { get; set; }
		 
		 
    }
}