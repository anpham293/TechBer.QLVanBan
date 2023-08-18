using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetUserDuAnForEditOutput
    {
		public CreateOrEditUserDuAnDto UserDuAn { get; set; }

		public string UserName { get; set;}

		public string DuAnName { get; set;}


    }
}