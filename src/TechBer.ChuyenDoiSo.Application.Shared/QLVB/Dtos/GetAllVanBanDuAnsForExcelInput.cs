using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllVanBanDuAnsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string KyHieuVanBanFilter { get; set; }

		public DateTime? MaxNgayBanHanhFilter { get; set; }
		public DateTime? MinNgayBanHanhFilter { get; set; }

		public string FileVanBanFilter { get; set; }


		 public string DuAnNameFilter { get; set; }

		 		 public string QuyTrinhDuAnNameFilter { get; set; }

		 
    }
}