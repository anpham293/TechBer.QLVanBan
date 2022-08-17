using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetTieuChiDanhGiaForEditOutput
    {
		public CreateOrEditTieuChiDanhGiaDto TieuChiDanhGia { get; set; }

		public string TieuChiDanhGiaName { get; set;}


    }
}