using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.VanBanDuAns
{
    public class CreateOrEditVanBanDuAnModalViewModel
    {
        public CreateOrEditVanBanDuAnDto VanBanDuAn { get; set; }

        public string DuAnName { get; set; }

        public string QuyTrinhDuAnName { get; set; }
        public string QuyetDinhSo { get; set; }


        public bool IsEditMode => VanBanDuAn.Id.HasValue;
    }
}