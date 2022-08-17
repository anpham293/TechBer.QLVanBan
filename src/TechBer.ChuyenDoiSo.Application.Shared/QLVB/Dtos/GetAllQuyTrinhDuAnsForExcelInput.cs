using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllQuyTrinhDuAnsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionsFilter { get; set; }

		public int? MaxSTTFilter { get; set; }
		public int? MinSTTFilter { get; set; }


		 public string LoaiDuAnNameFilter { get; set; }

		 
    }
}