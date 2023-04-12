
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class ThungHoSoDto : EntityDto
    {
		public string MaSo { get; set; }

		public string Ten { get; set; }

		public string MoTa { get; set; }

		public int TrangThai { get; set; }


		 public int? DayKeId { get; set; }

		 		 public int? DuAnId { get; set; }

		 
    }
}