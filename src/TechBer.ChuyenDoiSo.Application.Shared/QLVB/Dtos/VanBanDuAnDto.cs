
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class VanBanDuAnDto : EntityDto
    {
		public string Name { get; set; }

		public string KyHieuVanBan { get; set; }

		public DateTime NgayBanHanh { get; set; }

		public string FileVanBan { get; set; }


		 public int? DuAnId { get; set; }

		 		 public int? QuyTrinhDuAnId { get; set; }

		 
    }
}