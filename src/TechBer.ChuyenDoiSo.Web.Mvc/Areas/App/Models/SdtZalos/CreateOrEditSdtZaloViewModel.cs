using TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.SdtZalos
{
    public class CreateOrEditSdtZaloModalViewModel
    {
       public CreateOrEditSdtZaloDto SdtZalo { get; set; }

	   
       
	   public bool IsEditMode => SdtZalo.Id.HasValue;
    }
}