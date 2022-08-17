using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.MultiTenancy;

namespace TechBer.ChuyenDoiSo.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}