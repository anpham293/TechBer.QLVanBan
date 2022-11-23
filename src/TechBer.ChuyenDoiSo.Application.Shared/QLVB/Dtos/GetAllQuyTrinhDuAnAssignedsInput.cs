using Abp.Application.Services.Dto;
using System;

namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class GetAllQuyTrinhDuAnAssignedsInput : PagedAndSortedResultRequestDto
    {
        // public string Filter { get; set; }
        //
        // public string NameFilter { get; set; }
        //
        // public string DescriptionsFilter { get; set; }
        //
        // public int? MaxSTTFilter { get; set; }
        // public int? MinSTTFilter { get; set; }
        //
        // public int? MaxSoVanBanQuyDinhFilter { get; set; }
        // public int? MinSoVanBanQuyDinhFilter { get; set; }
        //
        // public string MaQuyTrinhFilter { get; set; }
        //
        //
        // public string LoaiDuAnNameFilter { get; set; }
        //
        // public string QuyTrinhDuAnNameFilter { get; set; }
        //
        // public string QuyTrinhDuAnAssignedNameFilter { get; set; }
        //
        // public string DuAnNameFilter { get; set; }
    }
    public class GetAllHoSoCanDuyetInput : PagedAndSortedResultRequestDto
    {
        public string TextFilter { get; set; }
        public string DuAnNameFilter { get; set; }
        public int? TrangThaiDuyetFilter { get; set; }
    }
}