using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.ChiTietThuHoies
{
    public class CreateOrEditChiTietThuHoiModalViewModel
    {
       public CreateOrEditChiTietThuHoiDto ChiTietThuHoi { get; set; }

	   		public string DanhMucThuHoiTen { get; set;}


       
	   public bool IsEditMode => ChiTietThuHoi.Id.HasValue;
    }
}