using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public interface ITraoDoiVanBanDuAnsExcelExporter
    {
        FileDto ExportToFile(List<GetTraoDoiVanBanDuAnForViewDto> traoDoiVanBanDuAns);
    }
}