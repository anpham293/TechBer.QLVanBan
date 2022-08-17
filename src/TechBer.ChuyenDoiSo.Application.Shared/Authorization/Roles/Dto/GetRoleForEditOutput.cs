using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Permissions.Dto;

namespace TechBer.ChuyenDoiSo.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}