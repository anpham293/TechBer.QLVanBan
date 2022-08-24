using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetQuyTrinhDuAnAssignedForEditOutput
    {
		public CreateOrEditQuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }

		public string LoaiDuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}

		public string QuyTrinhDuAnAssignedName { get; set;}

		public string DuAnName { get; set;}


    }
}