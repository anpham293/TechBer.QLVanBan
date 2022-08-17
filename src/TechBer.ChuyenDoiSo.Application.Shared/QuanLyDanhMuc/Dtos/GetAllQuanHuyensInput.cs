using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetAllQuanHuyensInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string MaFilter { get; set; }


		 public string TinhThanhNameFilter { get; set; }

		 
    }
}