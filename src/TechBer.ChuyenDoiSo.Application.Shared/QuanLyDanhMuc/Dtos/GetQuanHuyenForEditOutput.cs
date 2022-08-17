using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos
{
    public class GetQuanHuyenForEditOutput
    {
		public CreateOrEditQuanHuyenDto QuanHuyen { get; set; }

		public string TinhThanhName { get; set;}


    }
}