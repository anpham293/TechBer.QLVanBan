using TechBer.ChuyenDoiSo.QLVB.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.VanBanDuAns
{
    public class VanBanDuAnViewModel : GetVanBanDuAnForViewDto
    {
        public string guid{ get; set; }
        public string contentType{ get; set; }
        public string fileName{ get; set; }
    }
}