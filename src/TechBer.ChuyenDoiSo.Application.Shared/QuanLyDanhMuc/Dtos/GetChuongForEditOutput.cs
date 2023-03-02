using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetChuongForEditOutput
    {
		public CreateOrEditChuongDto Chuong { get; set; }

		public string CapQuanLyTen { get; set;}


    }
}