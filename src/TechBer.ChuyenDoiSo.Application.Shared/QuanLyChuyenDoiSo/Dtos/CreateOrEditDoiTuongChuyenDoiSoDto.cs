
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class CreateOrEditDoiTuongChuyenDoiSoDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public int Type { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}