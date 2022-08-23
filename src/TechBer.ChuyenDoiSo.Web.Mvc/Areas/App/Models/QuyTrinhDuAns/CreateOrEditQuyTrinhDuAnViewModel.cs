using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyTrinhDuAns
{
    public class CreateOrEditQuyTrinhDuAnModalViewModel
    {
       public CreateOrEditQuyTrinhDuAnDto QuyTrinhDuAn { get; set; }

	   		public string LoaiDuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}


       
	   public bool IsEditMode => QuyTrinhDuAn.Id.HasValue;
    }
}