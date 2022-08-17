using System;
using Abp.IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.Web.IdentityServer
{
    public static class IdentityServerRegistrar
    {
        public static void Register(IServiceCollection services, IConfigurationRoot configuration, Action<IdentityServerOptions> setupOptions)
        {
            services.AddIdentityServer(setupOptions)
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
                .AddAbpPersistedGrants<ChuyenDoiSoDbContext>()
                .AddAbpIdentityServer<User>();
        }

        public static void Register(IServiceCollection services, IConfigurationRoot configuration)
        {
            Register(services, configuration, options => { });
        }
    }
}
