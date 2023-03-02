
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditChuongDto : EntityDto<int?>
    {

		public string MaSo { get; set; }
		
		
		public string Ten { get; set; }
		
		
		 public int? CapQuanLyId { get; set; }
		 
		 
    }
}