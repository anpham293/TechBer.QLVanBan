using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Users.Dto;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}