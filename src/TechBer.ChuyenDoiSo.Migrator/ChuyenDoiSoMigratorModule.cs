using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using TechBer.ChuyenDoiSo.Configuration;
using TechBer.ChuyenDoiSo.EntityFrameworkCore;
using TechBer.ChuyenDoiSo.Migrator.DependencyInjection;

namespace TechBer.ChuyenDoiSo.Migrator
{
    [DependsOn(typeof(ChuyenDoiSoEntityFrameworkCoreModule))]
    public class ChuyenDoiSoMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ChuyenDoiSoMigratorModule(ChuyenDoiSoEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(ChuyenDoiSoMigratorModule).GetAssembly().GetDirectoryPathOrNull(), addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ChuyenDoiSoConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}