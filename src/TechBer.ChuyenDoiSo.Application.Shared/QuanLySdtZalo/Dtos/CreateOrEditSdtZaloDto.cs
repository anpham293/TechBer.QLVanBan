﻿
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos
{
    public class CreateOrEditSdtZaloDto : EntityDto<long?>
    {

		public string Ten { get; set; }
		
		
		public string Sdt { get; set; }
		
		

    }
}