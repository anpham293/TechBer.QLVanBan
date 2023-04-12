using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.ThungHoSos
{
    public class CreateOrEditThungHoSoModalViewModel
    {
       public CreateOrEditThungHoSoDto ThungHoSo { get; set; }

	   		public string DayKeMaSo { get; set;}

		public string DuAnName { get; set;}


       
	   public bool IsEditMode => ThungHoSo.Id.HasValue;
    }
}