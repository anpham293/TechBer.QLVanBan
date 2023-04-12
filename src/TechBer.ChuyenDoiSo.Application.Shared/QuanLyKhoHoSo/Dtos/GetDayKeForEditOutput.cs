using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class GetDayKeForEditOutput
    {
		public CreateOrEditDayKeDto DayKe { get; set; }

		public string PhongKhoMaSo { get; set;}


    }
}