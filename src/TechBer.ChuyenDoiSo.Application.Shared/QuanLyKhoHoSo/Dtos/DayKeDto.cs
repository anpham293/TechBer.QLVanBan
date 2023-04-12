
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class DayKeDto : EntityDto
    {
		public string MaSo { get; set; }

		public string Ten { get; set; }


		 public int? PhongKhoId { get; set; }

		 
    }
}