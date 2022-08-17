
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class TieuChiDanhGiaDto : EntityDto
    {
		public string Name { get; set; }

		public string SoThuTu { get; set; }

		public double DiemToiDa { get; set; }

		public string PhuongThucDanhGia { get; set; }

		public string TaiLieuGiaiTrinh { get; set; }

		public string GhiChu { get; set; }

		public int LoaiPhuLuc { get; set; }

		public int? SapXep { get; set; }

		 public int? ParentId { get; set; }

		 
    }
}