using TechBer.ChuyenDoiSo.QLVB.Dtos;
 using Abp.Extensions;
 
 namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.ChuyenHoSoGiaies
 {
     public class CreateOrEditChuyenHoSoGiayModalViewModel
     {
         public CreateOrEditChuyenHoSoGiayDto ChuyenHoSoGiay { get; set; }
 
         public string VanBanDuAnName { get; set; }
 
         public string UserName { get; set; }
         public int VanBanDuAnId { get; set; }
 
 
         public bool IsEditMode => ChuyenHoSoGiay.Id.HasValue;
     }
 }