
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class QuyTrinhDuAnDto : EntityDto
    {
		public string Name { get; set; }

		public string Descriptions { get; set; }

		public int STT { get; set; }
		public int SoVanBanQuyDinh { get; set; }

		public string MaQuyTrinh { get; set; }

		public string GhiChu { get; set; }


		 public int? LoaiDuAnId { get; set; }

		 		 public int? ParentId { get; set; }

		 
    }
    public class QuyTrinhDuAnHasMemberCountDto : QuyTrinhDuAnDto
    {
	    public int MemberCount { get; set; }
	    public int Mode { get; set; }
    }
}