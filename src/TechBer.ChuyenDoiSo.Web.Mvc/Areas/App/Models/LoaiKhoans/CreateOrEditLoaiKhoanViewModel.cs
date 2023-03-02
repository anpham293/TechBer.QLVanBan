using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.LoaiKhoans
{
    public class CreateOrEditLoaiKhoanModalViewModel
    {
       public CreateOrEditLoaiKhoanDto LoaiKhoan { get; set; }

	   
       
	   public bool IsEditMode => LoaiKhoan.Id.HasValue;
    }
}