using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo.Startup
{
    [DependsOn(typeof(ChuyenDoiSoCoreModule))]
    public class ChuyenDoiSoGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}