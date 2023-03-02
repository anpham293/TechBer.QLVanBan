using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public interface ILoaiKhoansExcelExporter
    {
        FileDto ExportToFile(List<GetLoaiKhoanForViewDto> loaiKhoans);
    }
}