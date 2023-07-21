using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class TraoDoiVanBanDuAnDto : EntityDto
    {
        public DateTime NgayGui { get; set; }

        public string NoiDung { get; set; }


        public long? UserId { get; set; }

        public int? VanBanDuAnId { get; set; }
        
        public string UserName { get; set;}
    }
}