
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditUserDuAnDto : EntityDto<long?>
    {

		public int TrangThai { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? DuAnId { get; set; }
		 
		 
    }
}