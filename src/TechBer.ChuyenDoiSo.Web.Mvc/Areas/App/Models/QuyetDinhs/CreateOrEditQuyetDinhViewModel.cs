using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyetDinhs
{
    public class CreateOrEditQuyetDinhModalViewModel
    {
       public CreateOrEditQuyetDinhDto QuyetDinh { get; set; }

	   
       
	   public bool IsEditMode => QuyetDinh.Id.HasValue;
    }
}