using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class GetAllThungHoSosForExcelInput
    {
		public string Filter { get; set; }

		public string MaSoFilter { get; set; }

		public string TenFilter { get; set; }

		public string MoTaFilter { get; set; }

		public int? MaxTrangThaiFilter { get; set; }
		public int? MinTrangThaiFilter { get; set; }


		 public string DayKeMaSoFilter { get; set; }

		 		 public string DuAnNameFilter { get; set; }

		 
    }
}