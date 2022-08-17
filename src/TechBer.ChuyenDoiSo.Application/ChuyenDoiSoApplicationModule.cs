using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TechBer.ChuyenDoiSo.Authorization;

namespace TechBer.ChuyenDoiSo
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(ChuyenDoiSoApplicationSharedModule),
        typeof(ChuyenDoiSoCoreModule)
        )]
    public class ChuyenDoiSoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoApplicationModule).GetAssembly());
        }
    }
}