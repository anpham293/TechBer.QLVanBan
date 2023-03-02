using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetAllLoaiKhoansForExcelInput
    {
		public string Filter { get; set; }

		public string MaSoFilter { get; set; }

		public string TenFilter { get; set; }

		public string GhiChuFilter { get; set; }



    }
}