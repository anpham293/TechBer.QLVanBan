﻿using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos
{
    public class GetAllPhongKhosInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string MaSoFilter { get; set; }

		public string TenFilter { get; set; }



    }
}