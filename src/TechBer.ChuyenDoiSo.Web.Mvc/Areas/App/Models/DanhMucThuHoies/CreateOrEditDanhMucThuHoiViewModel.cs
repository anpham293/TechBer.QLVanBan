using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DanhMucThuHoies
{
    public class CreateOrEditDanhMucThuHoiModalViewModel
    {
       public CreateOrEditDanhMucThuHoiDto DanhMucThuHoi { get; set; }

	   		public string DuAnThuHoiMaDATH { get; set;}


       
	   public bool IsEditMode => DanhMucThuHoi.Id.HasValue;
    }
}