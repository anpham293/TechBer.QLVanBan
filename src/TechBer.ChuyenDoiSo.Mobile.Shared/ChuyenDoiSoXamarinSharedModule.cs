using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo
{
    [DependsOn(typeof(ChuyenDoiSoClientModule), typeof(AbpAutoMapperModule))]
    public class ChuyenDoiSoXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoXamarinSharedModule).GetAssembly());
        }
    }
}