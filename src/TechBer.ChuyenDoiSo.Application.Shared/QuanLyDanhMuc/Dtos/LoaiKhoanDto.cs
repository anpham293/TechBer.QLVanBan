
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class LoaiKhoanDto : EntityDto
    {
		public string MaSo { get; set; }

		public string Ten { get; set; }

		public string GhiChu { get; set; }



    }
}