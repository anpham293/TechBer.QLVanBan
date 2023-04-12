using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Exporting
{
    public interface IThungHoSosExcelExporter
    {
        FileDto ExportToFile(List<GetThungHoSoForViewDto> thungHoSos);
    }
}