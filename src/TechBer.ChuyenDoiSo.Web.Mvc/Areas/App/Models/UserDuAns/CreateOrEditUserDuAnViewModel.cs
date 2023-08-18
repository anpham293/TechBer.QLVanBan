using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.UserDuAns
{
    public class CreateOrEditUserDuAnModalViewModel
    {
       public CreateOrEditUserDuAnDto UserDuAn { get; set; }

	   		public string UserName { get; set;}

		public string DuAnName { get; set;}


       
	   public bool IsEditMode => UserDuAn.Id.HasValue;
    }
}