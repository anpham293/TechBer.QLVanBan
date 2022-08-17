using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo
{
    [DependsOn(typeof(ChuyenDoiSoXamarinSharedModule))]
    public class ChuyenDoiSoXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoXamarinIosModule).GetAssembly());
        }
    }
}