using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Chuongs
{
    public class CreateOrEditChuongModalViewModel
    {
       public CreateOrEditChuongDto Chuong { get; set; }

	   		public string CapQuanLyTen { get; set;}


       
	   public bool IsEditMode => Chuong.Id.HasValue;
    }
}