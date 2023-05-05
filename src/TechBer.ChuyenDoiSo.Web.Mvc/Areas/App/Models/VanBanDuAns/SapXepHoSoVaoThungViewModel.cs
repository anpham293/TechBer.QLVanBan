using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Extensions;
using TechBer.ChuyenDoiSo.QLVB;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.VanBanDuAns
{
    public class SapXepHoSoVaoThungViewModel
    {
        public CreateOrEditVanBanDuAnDto VanBanDuAn { get; set; }
        
        public string ThungHoSoName { get; set; }
    } 
    public class SapXepHoSoVaoThungModalInput
    {
        public int VanBanDuAnId { get; set; }
    }
}