
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class ChuongDto : EntityDto
    {
		public string MaSo { get; set; }

		public string Ten { get; set; }


		 public int? CapQuanLyId { get; set; }

		 
    }
}