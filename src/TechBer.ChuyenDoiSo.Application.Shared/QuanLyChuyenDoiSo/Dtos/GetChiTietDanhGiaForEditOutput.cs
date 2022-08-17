using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetChiTietDanhGiaForEditOutput
    {
		public CreateOrEditChiTietDanhGiaDto ChiTietDanhGia { get; set; }

		public string TieuChiDanhGiaName { get; set;}

		public string DoiTuongChuyenDoiSoName { get; set;}


    }
}