using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class GetAllDayKesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string MaSoFilter { get; set; }

		public string TenFilter { get; set; }


		 public string PhongKhoMaSoFilter { get; set; }

		 
    }
}