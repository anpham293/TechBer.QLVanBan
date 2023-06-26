using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetBaoCaoVanBanDuAnForEditOutput
    {
		public CreateOrEditBaoCaoVanBanDuAnDto BaoCaoVanBanDuAn { get; set; }

		public string VanBanDuAnName { get; set;}

		public string UserName { get; set;}


    }
}