using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class CreateOrEditBaoCaoVanBanDuAnDto : EntityDto<int?>
    {
        public string NoiDungCongViec { get; set; }


        public string MoTaChiTiet { get; set; }


        public string FileBaoCao { get; set; }


        public int? VanBanDuAnId { get; set; }

        public long? UserId { get; set; }
    }
}