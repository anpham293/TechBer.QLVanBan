
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditLoaiDuAnDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		

    }
}