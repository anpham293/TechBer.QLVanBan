
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class QuanHuyenDto : EntityDto
    {
		public string Name { get; set; }

		public string Ma { get; set; }


		 public int TinhThanhId { get; set; }

		 
    }
}