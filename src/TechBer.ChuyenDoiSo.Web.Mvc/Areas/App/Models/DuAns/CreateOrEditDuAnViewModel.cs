using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DuAns
{
    public class CreateOrEditDuAnModalViewModel
    {
       public CreateOrEditDuAnDto DuAn { get; set; }

	   		public string LoaiDuAnName { get; set;}


       
	   public bool IsEditMode => DuAn.Id.HasValue;
    }
}