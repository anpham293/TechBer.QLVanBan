using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetTraoDoiVanBanDuAnForEditOutput
    {
		public CreateOrEditTraoDoiVanBanDuAnDto TraoDoiVanBanDuAn { get; set; }

		public string UserName { get; set;}

		public string VanBanDuAnName { get; set;}


    }
}