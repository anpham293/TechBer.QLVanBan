using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Exporting
{
    public interface IDoiTuongChuyenDoiSosExcelExporter
    {
        FileDto ExportToFile(List<GetDoiTuongChuyenDoiSoForViewDto> doiTuongChuyenDoiSos);
    }
}