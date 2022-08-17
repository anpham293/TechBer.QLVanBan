using Abp.Authorization;
using TechBer.ChuyenDoiSo.Authorization.Roles;
using TechBer.ChuyenDoiSo.Authorization.Users;

namespace TechBer.ChuyenDoiSo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
