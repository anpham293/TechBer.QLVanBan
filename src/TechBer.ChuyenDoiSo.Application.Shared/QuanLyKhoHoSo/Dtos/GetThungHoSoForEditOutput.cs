using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class GetThungHoSoForEditOutput
    {
		public CreateOrEditThungHoSoDto ThungHoSo { get; set; }

		public string DayKeMaSo { get; set;}

		public string DuAnName { get; set;}


    }
}