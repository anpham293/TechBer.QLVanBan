using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetAllQuanHuyensForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string MaFilter { get; set; }


		 public string TinhThanhNameFilter { get; set; }

		 
    }
}