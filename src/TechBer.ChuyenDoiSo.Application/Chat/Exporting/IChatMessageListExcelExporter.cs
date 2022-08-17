using System.Collections.Generic;
using Abp;
using TechBer.ChuyenDoiSo.Chat.Dto;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
