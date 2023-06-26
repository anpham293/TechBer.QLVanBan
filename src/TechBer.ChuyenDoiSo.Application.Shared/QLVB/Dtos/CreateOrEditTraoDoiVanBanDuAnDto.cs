
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditTraoDoiVanBanDuAnDto : EntityDto<int?>
    {

		public DateTime NgayGui { get; set; }
		
		
		public string NoiDung { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? VanBanDuAnId { get; set; }
		 
		 
    }
}