using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyTrinhDuAnAssigneds
{
    public class CreateOrEditQuyTrinhDuAnAssignedModalViewModel
    {
       public CreateOrEditQuyTrinhDuAnAssignedDto QuyTrinhDuAnAssigned { get; set; }

	   		public string LoaiDuAnName { get; set;}

		public string QuyTrinhDuAnName { get; set;}

		public string QuyTrinhDuAnAssignedName { get; set;}

		public string DuAnName { get; set;}


       
	   public bool IsEditMode => QuyTrinhDuAnAssigned.Id.HasValue;
    }
}