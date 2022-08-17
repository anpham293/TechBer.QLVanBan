
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class QuyTrinhDuAnDto : EntityDto
    {
		public string Name { get; set; }

		public string Descriptions { get; set; }


		 public int? LoaiDuAnId { get; set; }

		 
    }
}