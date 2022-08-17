using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace TechBer.ChuyenDoiSo.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
