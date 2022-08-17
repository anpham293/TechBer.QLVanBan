using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllDuAnsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionsFilter { get; set; }


		 public string LoaiDuAnNameFilter { get; set; }

		 
    }
}