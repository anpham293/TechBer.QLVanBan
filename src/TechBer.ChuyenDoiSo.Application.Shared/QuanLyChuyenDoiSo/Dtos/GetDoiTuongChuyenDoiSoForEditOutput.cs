using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetDoiTuongChuyenDoiSoForEditOutput
    {
		public CreateOrEditDoiTuongChuyenDoiSoDto DoiTuongChuyenDoiSo { get; set; }

		public string UserName { get; set;}


    }
}