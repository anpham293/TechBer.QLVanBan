using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TechBer.ChuyenDoiSo.Configure;
using TechBer.ChuyenDoiSo.Startup;
using TechBer.ChuyenDoiSo.Test.Base;

namespace TechBer.ChuyenDoiSo.GraphQL.Tests
{
    [DependsOn(
        typeof(ChuyenDoiSoGraphQLModule),
        typeof(ChuyenDoiSoTestBaseModule))]
    public class ChuyenDoiSoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoGraphQLTestModule).GetAssembly());
        }
    }
}