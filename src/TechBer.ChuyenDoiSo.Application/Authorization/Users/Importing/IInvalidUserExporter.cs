using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Users.Importing.Dto;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
