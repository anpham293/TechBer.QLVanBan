using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos
{
    public class GetAllSdtZalosForExcelInput
    {
		public string Filter { get; set; }

		public string TenFilter { get; set; }

		public string SdtFilter { get; set; }



    }
}