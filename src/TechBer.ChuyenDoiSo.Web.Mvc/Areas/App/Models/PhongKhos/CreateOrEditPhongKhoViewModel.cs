using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.PhongKhos
{
    public class CreateOrEditPhongKhoModalViewModel
    {
       public CreateOrEditPhongKhoDto PhongKho { get; set; }

	   
       
	   public bool IsEditMode => PhongKho.Id.HasValue;
    }
}