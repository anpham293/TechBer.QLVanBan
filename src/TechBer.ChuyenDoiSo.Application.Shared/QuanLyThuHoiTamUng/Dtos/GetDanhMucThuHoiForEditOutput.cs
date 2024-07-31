using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class GetDanhMucThuHoiForEditOutput
    {
		public CreateOrEditDanhMucThuHoiDto DanhMucThuHoi { get; set; }

		public string DuAnThuHoiMaDATH { get; set;}


    }
}