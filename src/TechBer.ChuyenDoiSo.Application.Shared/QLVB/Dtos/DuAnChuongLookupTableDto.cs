using Abp.Application.Services.Dto;
   
   namespace TechBer.ChuyenDoiSo.QLVB.Dtos
   {
       public class DuAnChuongLookupTableDto
       {
           public int Id { get; set; }
   
           public string MaSo { get; set; }
           public string Ten { get; set; }
           public int CapQuanLyId { get; set; }
       }
   }