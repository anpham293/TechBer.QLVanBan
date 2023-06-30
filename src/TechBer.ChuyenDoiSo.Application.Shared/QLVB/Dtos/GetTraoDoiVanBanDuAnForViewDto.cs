using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetTraoDoiVanBanDuAnForViewDto
    {
		public TraoDoiVanBanDuAnDto TraoDoiVanBanDuAn { get; set; }

		public string UserName { get; set;}

		public string VanBanDuAnName { get; set;}
    }

    public class GetHienThiTraoDoiDto
    {
	    public List<TraoDoiVanBanDuAnDto> ListTraoDoiVanBanDuAn { get; set; }
    }
}