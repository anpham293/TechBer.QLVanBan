
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
		 
		 
    }
}