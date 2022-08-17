using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetAllDoiTuongChuyenDoiSosInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		//public int? MaxTypeFilter { get; set; }
		//public int? MinTypeFilter { get; set; }

		public int? PhuLucFilter { get; set; }

		public string UserNameFilter { get; set; }

    }
}