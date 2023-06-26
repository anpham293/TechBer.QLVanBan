using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.TraoDoiVanBanDuAns
{
    public class CreateOrEditTraoDoiVanBanDuAnModalViewModel
    {
       public CreateOrEditTraoDoiVanBanDuAnDto TraoDoiVanBanDuAn { get; set; }

	   		public string UserName { get; set;}

		public string VanBanDuAnName { get; set;}


       
	   public bool IsEditMode => TraoDoiVanBanDuAn.Id.HasValue;
    }
}