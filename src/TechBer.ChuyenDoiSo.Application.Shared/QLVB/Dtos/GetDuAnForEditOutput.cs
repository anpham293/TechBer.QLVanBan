using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetDuAnForEditOutput
    {
		public CreateOrEditDuAnDto DuAn { get; set; }

		public string LoaiDuAnName { get; set;}
		public string ChuongName { get; set;}
		public string LoaiKhoanName { get; set;}


    }
}