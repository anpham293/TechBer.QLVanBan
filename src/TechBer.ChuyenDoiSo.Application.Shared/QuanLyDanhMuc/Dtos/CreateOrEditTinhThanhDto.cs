
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditTinhThanhDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		[Required]
		public string Ma { get; set; }
		
		

    }
}