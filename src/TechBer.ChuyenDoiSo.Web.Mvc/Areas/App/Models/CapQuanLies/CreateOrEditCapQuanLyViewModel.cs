using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.CapQuanLies
{
    public class CreateOrEditCapQuanLyModalViewModel
    {
       public CreateOrEditCapQuanLyDto CapQuanLy { get; set; }

	   
       
	   public bool IsEditMode => CapQuanLy.Id.HasValue;
    }
}