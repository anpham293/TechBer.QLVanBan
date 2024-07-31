using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting
{
    public interface IDuAnThuHoiesExcelExporter
    {
        FileDto ExportToFile(List<GetDuAnThuHoiForViewDto> duAnThuHoies);
    }
}