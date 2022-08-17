using TechBer.ChuyenDoiSo.QLVB.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.LoaiDuAns
{
    public class CreateOrEditLoaiDuAnModalViewModel
    {
       public CreateOrEditLoaiDuAnDto LoaiDuAn { get; set; }

	   
       
	   public bool IsEditMode => LoaiDuAn.Id.HasValue;
    }
}