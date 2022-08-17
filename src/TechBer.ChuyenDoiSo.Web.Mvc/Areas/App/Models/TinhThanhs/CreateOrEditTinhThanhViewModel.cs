using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.TinhThanhs
{
    public class CreateOrEditTinhThanhModalViewModel
    {
       public CreateOrEditTinhThanhDto TinhThanh { get; set; }

	   
       
	   public bool IsEditMode => TinhThanh.Id.HasValue;
    }
}