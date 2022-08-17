using System;
using System.IO;
using System.Linq;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Configuration.Startup;
using Abp.IdentityFramework;
using Abp.IO.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TechBer.ChuyenDoiSo.Web.Controllers
{
    public abstract class ChuyenDoiSoControllerBase : AbpController
    {
        protected ChuyenDoiSoControllerBase()
        {
            LocalizationSourceName = ChuyenDoiSoConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected void SetTenantIdCookie(int? tenantId)
        {
            var multiTenancyConfig = HttpContext.RequestServices.GetRequiredService<IMultiTenancyConfig>();
            Response.Cookies.Append(
                multiTenancyConfig.TenantIdResolveKey,
                tenantId?.ToString(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(5),
                    Path = "/"
                }
            );
        }

    }
}