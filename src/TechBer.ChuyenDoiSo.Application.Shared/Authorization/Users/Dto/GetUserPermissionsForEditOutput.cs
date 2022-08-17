using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Permissions.Dto;

namespace TechBer.ChuyenDoiSo.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}