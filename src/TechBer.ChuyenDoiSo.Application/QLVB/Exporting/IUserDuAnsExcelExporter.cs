using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public interface IUserDuAnsExcelExporter
    {
        FileDto ExportToFile(List<GetUserDuAnForViewDto> userDuAns);
    }
}