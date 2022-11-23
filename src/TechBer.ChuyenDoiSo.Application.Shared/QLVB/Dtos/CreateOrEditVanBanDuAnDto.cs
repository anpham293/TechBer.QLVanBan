using System;
 using Abp.Application.Services.Dto;
 using System.ComponentModel.DataAnnotations;
 
 namespace TechBer.ChuyenDoiSo.QLVB.Dtos
 {
     public class CreateOrEditVanBanDuAnDto : EntityDto<int?>
     {
         [Required]
         [StringLength(VanBanDuAnConsts.MaxNameLength, MinimumLength = VanBanDuAnConsts.MinNameLength)]
         public string Name { get; set; }
 
 
         [Required]
         [StringLength(VanBanDuAnConsts.MaxKyHieuVanBanLength, MinimumLength = VanBanDuAnConsts.MinKyHieuVanBanLength)]
         public string KyHieuVanBan { get; set; }
 
 
         public DateTime NgayBanHanh { get; set; }
 
 
         public string FileVanBan { get; set; }
 
 
         public int? DuAnId { get; set; }
 
         public int? QuyTrinhDuAnId { get; set; }
 
         public string UploadedFileToken { get; set; }
         public string FileName { get; set; }
         public string ContentType { get; set; }
         
         public string ViTriLuuTru { get; set; }
         public decimal SoTienThanhToan { get; set; }
     }
 }