﻿using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}