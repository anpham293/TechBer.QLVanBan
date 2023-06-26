using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllTraoDoiVanBanDuAnsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxNgayGuiFilter { get; set; }
		public DateTime? MinNgayGuiFilter { get; set; }

		public string NoiDungFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string VanBanDuAnNameFilter { get; set; }

		 
    }
}