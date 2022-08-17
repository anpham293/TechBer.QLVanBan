using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.VanBanDuAns
{
    public class VanBanDuAnsViewModel
    {
		public string FilterText { get; set; }
        public DuAn DuAnId { get; set; }
        public List<DuAnDto> ListDuAn { get; set; }
    }
}