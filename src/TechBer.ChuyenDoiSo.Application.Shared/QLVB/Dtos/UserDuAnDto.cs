
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class UserDuAnDto : EntityDto<long>
    {
		public int TrangThai { get; set; }


		 public long? UserId { get; set; }

		 		 public int? DuAnId { get; set; }

		 
    }
}