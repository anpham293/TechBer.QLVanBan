
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class QuyTrinhDuAnAssignedDto : EntityDto<long>
    {
		public string Name { get; set; }

		public string Descriptions { get; set; }

		public int STT { get; set; }

		public int SoVanBanQuyDinh { get; set; }

		public string MaQuyTrinh { get; set; }


		 public int? LoaiDuAnId { get; set; }

		 		 public int? QuyTrinhDuAnId { get; set; }

		 		 public long? ParentId { get; set; }

		 		 public int? DuAnId { get; set; }

		 
    }
    
    public class QuyTrinhDuAnAssignedHasMemberCountDto : QuyTrinhDuAnAssignedDto
    {
	    public int MemberCount { get; set; }
	    public int Mode { get; set; }
    }
}