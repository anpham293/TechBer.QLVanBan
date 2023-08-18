using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllUserDuAnsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTrangThaiFilter { get; set; }
		public int? MinTrangThaiFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string DuAnNameFilter { get; set; }

		 
    }
}