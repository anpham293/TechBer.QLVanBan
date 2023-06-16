using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetVanBanDuAnForEditOutput
    {
		public CreateOrEditVanBanDuAnDto VanBanDuAn { get; set; }

		public string DuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}
		public string QuyetDinhSo { get; set; }
    }
    public class GetSapXepHoSoVaoThungOutput
    {
	    public CreateOrEditVanBanDuAnDto VanBanDuAn { get; set; }
        
	    public string ThungHoSoName { get; set; }
    }
}