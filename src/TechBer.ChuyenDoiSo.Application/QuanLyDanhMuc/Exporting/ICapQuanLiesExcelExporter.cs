using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public interface ICapQuanLiesExcelExporter
    {
        FileDto ExportToFile(List<GetCapQuanLyForViewDto> capQuanLies);
    }
}