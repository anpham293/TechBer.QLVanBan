using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public interface IQuyTrinhDuAnsExcelExporter
    {
        FileDto ExportToFile(List<GetQuyTrinhDuAnForViewDto> quyTrinhDuAns);
    }
}