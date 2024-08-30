
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos
{
    public class SdtZaloDto : EntityDto<long>
    {
		public string Ten { get; set; }

		public string Sdt { get; set; }



    }
}