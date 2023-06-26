using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllBaoCaoVanBanDuAnsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NoiDungCongViecFilter { get; set; }

		public string MoTaChiTietFilter { get; set; }

		public string FileBaoCaoFilter { get; set; }


		 public string VanBanDuAnNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}