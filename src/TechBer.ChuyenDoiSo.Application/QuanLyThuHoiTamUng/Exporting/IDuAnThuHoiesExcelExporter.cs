using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting
{
    public interface IDuAnThuHoiesExcelExporter
    {
        FileDto ExportToFile(List<GetDuAnThuHoiForViewDto> duAnThuHoies);
        FileDto BaoCaoDuAnThuHoi_ExportToFile(BaoCaoDuAnThuHoi_ExportToFileDto baoCao);
    }
}