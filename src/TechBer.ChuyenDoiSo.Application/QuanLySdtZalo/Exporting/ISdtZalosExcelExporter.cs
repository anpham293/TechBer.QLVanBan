using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo.Exporting
{
    public interface ISdtZalosExcelExporter
    {
        FileDto ExportToFile(List<GetSdtZaloForViewDto> sdtZalos);
    }
}