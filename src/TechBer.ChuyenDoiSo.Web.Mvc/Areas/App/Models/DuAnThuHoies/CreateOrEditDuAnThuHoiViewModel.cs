using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DuAnThuHoies
{
    public class CreateOrEditDuAnThuHoiModalViewModel
    {
       public CreateOrEditDuAnThuHoiDto DuAnThuHoi { get; set; }

	   
       
	   public bool IsEditMode => DuAnThuHoi.Id.HasValue;
    }
}