using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetChiTietThuHoiForEditOutput
    {
		public CreateOrEditChiTietThuHoiDto ChiTietThuHoi { get; set; }

		public string DanhMucThuHoiTen { get; set;}


    }
}