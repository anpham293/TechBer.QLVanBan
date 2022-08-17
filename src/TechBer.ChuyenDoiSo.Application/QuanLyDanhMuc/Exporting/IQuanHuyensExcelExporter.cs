using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public interface IQuanHuyensExcelExporter
    {
        FileDto ExportToFile(List<GetQuanHuyenForViewDto> quanHuyens);
    }
}