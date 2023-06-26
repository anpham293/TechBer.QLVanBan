using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.BaoCaoVanBanDuAns
{
    public class CreateOrEditBaoCaoVanBanDuAnModalViewModel
    {
       public CreateOrEditBaoCaoVanBanDuAnDto BaoCaoVanBanDuAn { get; set; }

	   		public string VanBanDuAnName { get; set;}

		public string UserName { get; set;}


       
	   public bool IsEditMode => BaoCaoVanBanDuAn.Id.HasValue;
    }
}