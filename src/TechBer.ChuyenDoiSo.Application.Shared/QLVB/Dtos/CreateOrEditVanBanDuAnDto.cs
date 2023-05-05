using System;
 using Abp.Application.Services.Dto;
 using System.ComponentModel.DataAnnotations;
 
 namespace TechBer.ChuyenDoiSo.QLVB.Dtos
 {
     public class CreateOrEditVanBanDuAnDto : EntityDto<int?>
     {
         public string Name { get; set; }
         
         public string KyHieuVanBan { get; set; }
 
 
         public DateTime NgayBanHanh { get; set; }
 
 
         public string FileVanBan { get; set; }
 
 
         public int? DuAnId { get; set; }
 
         public int? QuyTrinhDuAnId { get; set; }
 
         public string UploadedFileToken { get; set; }
         public string FileName { get; set; }
         public string ContentType { get; set; }
         
         public decimal SoTienThanhToan { get; set; }
         
         public int SoLuongVanBanGiay { get; set; }
         public int? ThungHoSoId { get; set; }
     }
 }