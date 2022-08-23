using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetQuyTrinhDuAnForEditOutput
    {
		public CreateOrEditQuyTrinhDuAnDto QuyTrinhDuAn { get; set; }

		public string LoaiDuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}


    }
}