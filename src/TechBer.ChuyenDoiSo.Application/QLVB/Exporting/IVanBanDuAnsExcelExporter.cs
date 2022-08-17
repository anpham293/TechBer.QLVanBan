using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public interface IVanBanDuAnsExcelExporter
    {
        FileDto ExportToFile(List<GetVanBanDuAnForViewDto> vanBanDuAns);
    }
}