
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class LoaiDuAnDto : EntityDto
    {
		public string Name { get; set; }


		 public long? OrganizationUnitId { get; set; }

		 
    }
}