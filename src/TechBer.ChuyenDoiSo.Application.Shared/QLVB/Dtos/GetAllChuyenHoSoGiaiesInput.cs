using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllChuyenHoSoGiaiesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxNguoiChuyenIdFilter { get; set; }
		public int? MinNguoiChuyenIdFilter { get; set; }

		public DateTime? MaxThoiGianChuyenFilter { get; set; }
		public DateTime? MinThoiGianChuyenFilter { get; set; }

		public int? MaxSoLuongFilter { get; set; }
		public int? MinSoLuongFilter { get; set; }

		public int? MaxTrangThaiFilter { get; set; }
		public int? MinTrangThaiFilter { get; set; }

		public DateTime? MaxThoiGianNhanFilter { get; set; }
		public DateTime? MinThoiGianNhanFilter { get; set; }


		 public string VanBanDuAnNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}