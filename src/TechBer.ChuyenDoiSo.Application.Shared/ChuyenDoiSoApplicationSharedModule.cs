using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo
{
    [DependsOn(typeof(ChuyenDoiSoCoreSharedModule))]
    public class ChuyenDoiSoApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoApplicationSharedModule).GetAssembly());
        }
    }
}