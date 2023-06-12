using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllQuyetDinhsForExcelInput
    {
		public string Filter { get; set; }

		public string SoFilter { get; set; }

		public string TenFilter { get; set; }

		public DateTime? MaxNgayBanHanhFilter { get; set; }
		public DateTime? MinNgayBanHanhFilter { get; set; }

		public string FileQuyetDinhFilter { get; set; }

		public int? MaxTrangThaiFilter { get; set; }
		public int? MinTrangThaiFilter { get; set; }



    }
}