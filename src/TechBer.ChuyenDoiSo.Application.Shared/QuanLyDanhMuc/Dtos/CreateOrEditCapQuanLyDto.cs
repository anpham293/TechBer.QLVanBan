
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditCapQuanLyDto : EntityDto<int?>
    {

		public string Ten { get; set; }
		
		

    }
}