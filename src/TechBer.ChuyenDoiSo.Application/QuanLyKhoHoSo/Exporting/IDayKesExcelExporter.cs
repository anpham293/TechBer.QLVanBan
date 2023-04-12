using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Exporting
{
    public interface IDayKesExcelExporter
    {
        FileDto ExportToFile(List<GetDayKeForViewDto> dayKes);
    }
}