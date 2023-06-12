
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class QuyetDinhDto : EntityDto
    {
		public string So { get; set; }

		public string Ten { get; set; }

		public DateTime NgayBanHanh { get; set; }

		public string FileQuyetDinh { get; set; }

		public int TrangThai { get; set; }



    }
}