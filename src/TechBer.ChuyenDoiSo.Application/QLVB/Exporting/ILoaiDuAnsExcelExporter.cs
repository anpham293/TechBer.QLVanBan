using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public interface ILoaiDuAnsExcelExporter
    {
        FileDto ExportToFile(List<GetLoaiDuAnForViewDto> loaiDuAns);
    }
}