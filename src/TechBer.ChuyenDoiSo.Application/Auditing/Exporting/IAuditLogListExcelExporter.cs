using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Auditing.Dto;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
