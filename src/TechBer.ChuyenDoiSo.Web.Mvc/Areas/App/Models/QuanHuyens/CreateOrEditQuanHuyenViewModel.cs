using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuanHuyens
{
    public class CreateOrEditQuanHuyenModalViewModel
    {
       public CreateOrEditQuanHuyenDto QuanHuyen { get; set; }

	   		public string TinhThanhName { get; set;}


       
	   public bool IsEditMode => QuanHuyen.Id.HasValue;
    }
}