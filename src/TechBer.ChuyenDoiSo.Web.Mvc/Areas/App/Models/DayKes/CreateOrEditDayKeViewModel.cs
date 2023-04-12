using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DayKes
{
    public class CreateOrEditDayKeModalViewModel
    {
       public CreateOrEditDayKeDto DayKe { get; set; }

	   		public string PhongKhoMaSo { get; set;}


       
	   public bool IsEditMode => DayKe.Id.HasValue;
    }
}